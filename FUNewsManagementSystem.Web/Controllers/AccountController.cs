using FUNewsManagementSystem.Services.Services;
using FUNewsManagementSystem.Web.Models;
using FUNewsManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers;

public sealed class AccountController : Controller
{
    private readonly AuthService _authService;

    public AccountController(AuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var account = _authService.Authenticate(model.Email, model.Password);
        if (account is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        HttpContext.Session.SetInt32(SessionKeys.AccountId, account.AccountId);
        HttpContext.Session.SetString(SessionKeys.AccountName, account.AccountName);
        HttpContext.Session.SetString(SessionKeys.AccountEmail, account.AccountEmail);
        HttpContext.Session.SetString(SessionKeys.AccountRole, account.AccountRole.ToString());

        return account.AccountRole switch
        {
            Data.Entities.AccountRole.Admin => RedirectToAction("Accounts", "Admin"),
            Data.Entities.AccountRole.Staff => RedirectToAction("Index", "News"),
            _ => RedirectToAction("Index", "Home")
        };
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() => View();
}
