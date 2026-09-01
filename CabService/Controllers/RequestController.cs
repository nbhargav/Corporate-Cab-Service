using CabService.Data;
using CabService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabService.Controllers
{
    /// <summary>
    /// Employee-facing controller: submit vehicle requests, track status, leave feedback.
    /// </summary>
    public class RequestController : Controller
    {
        private readonly CabServiceContext _context;

        public RequestController(CabServiceContext context)
        {
            _context = context;
        }

        private int? CurrentEmployeeId => HttpContext.Session.GetInt32("EmployeeId");

        public async Task<IActionResult> Dashboard()
        {
            if (CurrentEmployeeId == null) return RedirectToAction("Login", "Account");

            var myRequests = await _context.VehicleRequests
                .Include(r => r.Vehicle)
                .Include(r => r.Assignment)
                .Where(r => r.EmployeeId == CurrentEmployeeId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(myRequests);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleRequest model)
        {
            if (CurrentEmployeeId == null) return RedirectToAction("Login", "Account");

            if (model.RequiredTo <= model.RequiredFrom)
            {
                ModelState.AddModelError(string.Empty, "'Required To' must be after 'Required From'.");
                return View(model);
            }

            model.EmployeeId = CurrentEmployeeId.Value;
            model.Status = RequestStatus.Pending;
            model.CreatedAt = DateTime.UtcNow;

            _context.VehicleRequests.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult SubmitFeedback(int vehicleId)
        {
            ViewBag.VehicleId = vehicleId;
            return View(new Feedback { VehicleId = vehicleId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFeedback(Feedback model)
        {
            if (CurrentEmployeeId == null) return RedirectToAction("Login", "Account");

            model.EmployeeId = CurrentEmployeeId.Value;
            model.SubmittedAt = DateTime.UtcNow;

            _context.FeedbackEntries.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }
    }
}
