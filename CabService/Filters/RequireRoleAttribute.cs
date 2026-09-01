using CabService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CabService.Filters
{
    /// <summary>
    /// Guards a controller/action to a specific role, backed by the session set
    /// at login (AccountController.Login). Redirects unauthenticated or
    /// wrong-role users back to the login page rather than throwing.
    /// </summary>
    public class RequireRoleAttribute : ActionFilterAttribute
    {
        private readonly UserRole _role;

        public RequireRoleAttribute(UserRole role)
        {
            _role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var role = session.GetString("Role");

            if (role == null || role != _role.ToString())
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
