using FUNewsManagementSystem.Data.Entities;
using FUNewsManagementSystem.Data.Repositories;
using FUNewsManagementSystem.Services.Services;
using FUNewsManagementSystem.Web.Hubs;
using FUNewsManagementSystem.Web.Models;
using FUNewsManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Web.Controllers;

public sealed class NewsController : Controller
{
    private readonly NewsArticleRepository _news;
    private readonly CategoryRepository _categories;
    private readonly TagRepository _tags;
    private readonly NewsService _newsService;
    private readonly IHubContext<NewsHub> _newsHub;

    public NewsController(NewsArticleRepository news, CategoryRepository categories, TagRepository tags, NewsService newsService, IHubContext<NewsHub> newsHub)
    {
        _news = news;
        _categories = categories;
        _tags = tags;
        _newsService = newsService;
        _newsHub = newsHub;
    }

    public IActionResult Index(string? search)
    {
        var guard = this.RequireRole(AccountRole.Staff);
        if (guard is not EmptyResult) return guard;

        ViewBag.Search = search;
        PopulateLists();
        return View(_newsService.Search(search));
    }

    public IActionResult History(string? search)
    {
        var guard = this.RequireRole(AccountRole.Staff);
        if (guard is not EmptyResult) return guard;

        ViewBag.Search = search;
        PopulateLists();
        ViewBag.IsHistory = true;
        return View("Index", _newsService.Search(search, createdById: this.CurrentAccountId()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(NewsEditViewModel model)
    {
        var guard = this.RequireRole(AccountRole.Staff);
        if (guard is not EmptyResult) return guard;

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "News article data is invalid.";
            return RedirectToAction(nameof(Index));
        }

        var accountId = this.CurrentAccountId();
        var article = new NewsArticle
        {
            NewsArticleId = model.NewsArticleId ?? string.Empty,
            NewsTitle = model.NewsTitle,
            Headline = model.Headline,
            CreatedDate = model.CreatedDate,
            NewsContent = model.NewsContent,
            NewsSource = model.NewsSource,
            CategoryId = model.CategoryId,
            NewsStatus = model.NewsStatus,
            CreatedById = accountId,
            UpdatedById = accountId,
            TagIds = model.TagIds
        };

        var isCreate = string.IsNullOrWhiteSpace(model.NewsArticleId);
        if (isCreate)
        {
            _news.Add(article);
        }
        else
        {
            var existing = _news.GetById(model.NewsArticleId!);
            if (existing is null) return NotFound();
            article.CreatedById = existing.CreatedById;
            _news.Update(article);
        }

        await _newsHub.Clients.All.SendAsync("newsChanged", isCreate ? "created" : "updated", article.NewsArticleId);
        TempData["Message"] = "News article saved.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var guard = this.RequireRole(AccountRole.Staff);
        if (guard is not EmptyResult) return guard;

        var deleted = _news.Delete(id);
        if (deleted)
        {
            await _newsHub.Clients.All.SendAsync("newsChanged", "deleted", id);
        }

        TempData[deleted ? "Message" : "Error"] = _news.GetById(id) is null ? "News article deleted." : "News article was not found.";
        return RedirectToAction(nameof(Index));
    }

    private void PopulateLists()
    {
        ViewBag.Categories = new SelectList(_categories.GetAll().Where(c => c.IsActive), "CategoryId", "CategoryName");
        ViewBag.CategoryData = _categories.GetAll().Where(c => c.IsActive).ToList();
        ViewBag.Tags = _tags.GetAll();
    }
}
