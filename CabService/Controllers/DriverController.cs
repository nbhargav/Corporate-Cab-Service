using CabService.Data;
using CabService.Filters;
using CabService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabService.Controllers
{
    [RequireRole(UserRole.Driver)]
    public class DriverController : Controller
    {
        private readonly CabServiceContext _context;

        public DriverController(CabServiceContext context)
        {
            _context = context;
        }

        private int? CurrentDriverId => HttpContext.Session.GetInt32("EmployeeId");

        public async Task<IActionResult> Dashboard()
        {
            var assignedVehicles = await _context.Vehicles
                .Where(v => v.DriverId == CurrentDriverId)
                .ToListAsync();

            return View(assignedVehicles);
        }

        [HttpGet]
        public IActionResult LogFuel(int vehicleId)
        {
            return View(new FuelLog { VehicleId = vehicleId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogFuel(FuelLog model)
        {
            if (CurrentDriverId == null) return RedirectToAction("Login", "Account");

            model.DriverId = CurrentDriverId.Value;
            model.LogDate = DateTime.UtcNow;

            _context.FuelLogs.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }
    }
}
