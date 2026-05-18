using FUNewsManagementSystem.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers;

public sealed class HomeController : Controller
{
    private readonly NewsService _newsService;

    public HomeController(NewsService newsService)
    {
        _newsService = newsService;
    }

    public IActionResult Index(string? search)
    {
        ViewBag.Search = search;
        return View(_newsService.Search(search, activeOnly: true));
    }

    public IActionResult Details(string id)
    {
        var article = _newsService.GetDisplay(id);
        return article is null || !article.Article.NewsStatus ? NotFound() : View(article);
    }

    public IActionResult Privacy() => View();
}
