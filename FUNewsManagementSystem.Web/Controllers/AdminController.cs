using FUNewsManagementSystem.Data.Entities;
using FUNewsManagementSystem.Data.Repositories;
using FUNewsManagementSystem.Services.Services;
using FUNewsManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers;

public sealed class AdminController : Controller
{
    private readonly AccountRepository _accounts;
    private readonly NewsService _newsService;

    public AdminController(AccountRepository accounts, NewsService newsService)
    {
        _accounts = accounts;
        _newsService = newsService;
    }

    public IActionResult Accounts(string? search)
    {
        var guard = this.RequireRole(AccountRole.Admin);
        if (guard is not EmptyResult) return guard;

        ViewBag.Search = search;
        var accounts = _accounts.GetAll().AsEnumerable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            accounts = accounts.Where(a => a.AccountName.Contains(search, StringComparison.OrdinalIgnoreCase) || a.AccountEmail.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return View(accounts.ToList());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SaveAccount(SystemAccount account)
    {
        var guard = this.RequireRole(AccountRole.Admin);
        if (guard is not EmptyResult) return guard;

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Account data is invalid.";
            return RedirectToAction(nameof(Accounts));
        }

        if (account.AccountId == 0)
        {
            _accounts.Add(account);
        }
        else
        {
            _accounts.Update(account);
        }

        TempData["Message"] = "Account saved.";
        return RedirectToAction(nameof(Accounts));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteAccount(short id)
    {
        var guard = this.RequireRole(AccountRole.Admin);
        if (guard is not EmptyResult) return guard;

        TempData[_accounts.Delete(id) ? "Message" : "Error"] = _accounts.GetById(id) is null ? "Account deleted." : "Cannot delete an account used by news articles.";
        return RedirectToAction(nameof(Accounts));
    }

    public IActionResult Report(DateTime? startDate, DateTime? endDate)
    {
        var guard = this.RequireRole(AccountRole.Admin);
        if (guard is not EmptyResult) return guard;

        var start = startDate ?? DateTime.Today.AddMonths(-1);
        var end = endDate ?? DateTime.Today;
        ViewBag.StartDate = start.ToString("yyyy-MM-dd");
        ViewBag.EndDate = end.ToString("yyyy-MM-dd");
        return View(_newsService.Report(start, end));
    }
}
