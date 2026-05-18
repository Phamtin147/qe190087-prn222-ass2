using FUNewsManagementSystem.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Models;

public static class ControllerRoleExtensions
{
    public static bool IsSignedIn(this Controller controller) => controller.HttpContext.Session.GetString(SessionKeys.AccountRole) is not null;

    public static AccountRole? CurrentRole(this Controller controller)
    {
        var value = controller.HttpContext.Session.GetString(SessionKeys.AccountRole);
        return Enum.TryParse<AccountRole>(value, out var role) ? role : null;
    }

    public static short CurrentAccountId(this Controller controller) => (short)(controller.HttpContext.Session.GetInt32(SessionKeys.AccountId) ?? -1);

    public static IActionResult RequireRole(this Controller controller, params AccountRole[] roles)
    {
        var current = controller.CurrentRole();
        if (current is null)
        {
            return controller.RedirectToAction("Login", "Account");
        }

        if (!roles.Contains(current.Value))
        {
            return controller.RedirectToAction("AccessDenied", "Account");
        }

        return new EmptyResult();
    }
}
