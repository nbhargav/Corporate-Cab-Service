using CabService.Data;
using CabService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabService.Controllers
{
    public class AccountController : Controller
    {
        private readonly CabServiceContext _context;

        public AccountController(CabServiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View();
            }

            // Session-based auth: store minimal identity, role drives which
            // dashboard/permissions the user sees (see [RoleGuard] usage in other controllers).
            HttpContext.Session.SetInt32("EmployeeId", user.EmployeeId);
            HttpContext.Session.SetString("Role", user.Role.ToString());
            HttpContext.Session.SetString("FullName", user.FullName);

            return user.Role switch
            {
                UserRole.Admin => RedirectToAction("Dashboard", "Admin"),
                UserRole.Driver => RedirectToAction("Dashboard", "Driver"),
                _ => RedirectToAction("Dashboard", "Request")
            };
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Employee model, string password)
        {
            if (!ModelState.IsValid) return View(model);

            bool exists = await _context.Employees.AnyAsync(e => e.Username == model.Username);
            if (exists)
            {
                ModelState.AddModelError(string.Empty, "Username already taken.");
                return View(model);
            }

            model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            _context.Employees.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
