using CabService.Data;
using CabService.Filters;
using CabService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabService.Controllers
{
    [RequireRole(UserRole.Admin)]
    public class AdminController : Controller
    {
        private readonly CabServiceContext _context;

        public AdminController(CabServiceContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var pendingRequests = await _context.VehicleRequests
                .Include(r => r.Employee)
                .Where(r => r.Status == RequestStatus.Pending)
                .OrderBy(r => r.RequiredFrom)
                .ToListAsync();

            ViewBag.AvailableVehicles = await _context.Vehicles
                .Where(v => v.Status == VehicleStatus.Available)
                .ToListAsync();

            return View(pendingRequests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignVehicle(int requestId, int vehicleId, int? driverId)
        {
            var request = await _context.VehicleRequests.FindAsync(requestId);
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (request == null || vehicle == null) return NotFound();

            request.Status = RequestStatus.Approved;
            request.VehicleId = vehicleId;
            vehicle.Status = VehicleStatus.Assigned;

            var assignment = new AssignedVehicle
            {
                RequestId = requestId,
                VehicleId = vehicleId,
                DriverId = driverId,
                AssignedByAdminUsername = HttpContext.Session.GetString("FullName") ?? "admin",
                AssignedAt = DateTime.UtcNow
            };

            _context.AssignedVehicles.Add(assignment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var request = await _context.VehicleRequests.FindAsync(requestId);
            if (request == null) return NotFound();

            request.Status = RequestStatus.Rejected;
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        /// <summary>Fuel usage report across all vehicles, for the admin "reports" section.</summary>
        public async Task<IActionResult> FuelReport()
        {
            var report = await _context.FuelLogs
                .Include(f => f.Vehicle)
                .GroupBy(f => f.Vehicle!.RegistrationNumber)
                .Select(g => new
                {
                    Vehicle = g.Key,
                    TotalLiters = g.Sum(x => x.FuelQuantityLiters),
                    TotalCost = g.Sum(x => x.Cost)
                })
                .ToListAsync();

            return View(report);
        }

        public async Task<IActionResult> FeedbackReport()
        {
            var feedback = await _context.FeedbackEntries
                .Include(f => f.Employee)
                .Include(f => f.Vehicle)
                .OrderByDescending(f => f.SubmittedAt)
                .ToListAsync();

            return View(feedback);
        }
    }
}
