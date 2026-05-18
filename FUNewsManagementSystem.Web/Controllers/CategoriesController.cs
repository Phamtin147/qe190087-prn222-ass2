using FUNewsManagementSystem.Data.Entities;
using FUNewsManagementSystem.Data.Repositories;
using FUNewsManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers;

public sealed class CategoriesController : Controller
{
    private readonly CategoryRepository _categories;

    public CategoriesController(CategoryRepository categories)
    {
        _categories = categories;
    }

    public IActionResult Index(string? search)
    {
        var guard = this.RequireRole(AccountRole.Staff);
        if (guard is not EmptyResult) return guard;

        ViewBag.Search = search;
        var categories = _categories.GetAll().AsEnumerable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            categories = categories.Where(c =>
                c.CategoryName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                c.CategoryDescription.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return View(categories.ToList());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Save(Category category)
    {
        var guard = this.RequireRole(AccountRole.Staff);
        if (guard is not EmptyResult) return guard;

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Category data is invalid.";
            return RedirectToAction(nameof(Index));
        }

        if (category.ParentCategoryId == 0)
        {
            category.ParentCategoryId = null;
        }

        if (category.CategoryId == 0)
        {
            _categories.Add(category);
        }
        else
        {
            _categories.Update(category);
        }

        TempData["Message"] = "Category saved.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(short id)
    {
        var guard = this.RequireRole(AccountRole.Staff);
        if (guard is not EmptyResult) return guard;

        TempData[_categories.Delete(id) ? "Message" : "Error"] = _categories.GetById(id) is null ? "Category deleted." : "Cannot delete a category used by news articles.";
        return RedirectToAction(nameof(Index));
    }
}
