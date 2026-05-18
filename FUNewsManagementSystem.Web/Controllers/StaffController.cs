using FUNewsManagementSystem.Data.Entities;
using FUNewsManagementSystem.Data.Repositories;
using FUNewsManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers;

public sealed class StaffController : Controller
{
    private readonly AccountRepository _accounts;

    public StaffController(AccountRepository accounts)
    {
        _accounts = accounts;
    }

    public IActionResult Profile()
    {
        var guard = this.RequireRole(AccountRole.Staff);
        if (guard is not EmptyResult) return guard;

        var account = _accounts.GetById(this.CurrentAccountId());
        return account is null ? NotFound() : View(account);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Profile(SystemAccount account)
    {
        var guard = this.RequireRole(AccountRole.Staff);
        if (guard is not EmptyResult) return guard;

        var existing = _accounts.GetById(this.CurrentAccountId());
        if (existing is null) return NotFound();

        existing.AccountName = account.AccountName;
        existing.AccountEmail = account.AccountEmail;
        existing.AccountPassword = account.AccountPassword;
        _accounts.Update(existing);

        HttpContext.Session.SetString(SessionKeys.AccountName, existing.AccountName);
        HttpContext.Session.SetString(SessionKeys.AccountEmail, existing.AccountEmail);
        TempData["Message"] = "Profile updated.";
        return RedirectToAction(nameof(Profile));
    }
}
