using FUNewsManagementSystem.Data.Entities;
using FUNewsManagementSystem.Data.Repositories;
using FUNewsManagementSystem.Services.Models;

namespace FUNewsManagementSystem.Services.Services;

public sealed class NewsService
{
    private readonly NewsArticleRepository _news;
    private readonly CategoryRepository _categories;
    private readonly AccountRepository _accounts;
    private readonly TagRepository _tags;

    public NewsService(NewsArticleRepository news, CategoryRepository categories, AccountRepository accounts, TagRepository tags)
    {
        _news = news;
        _categories = categories;
        _accounts = accounts;
        _tags = tags;
    }

    public IReadOnlyList<NewsArticleDisplay> Search(string? keyword, bool activeOnly = false, short? createdById = null)
    {
        var query = _news.GetAll().AsEnumerable();

        if (activeOnly)
        {
            query = query.Where(n => n.NewsStatus);
        }

        if (createdById.HasValue)
        {
            query = query.Where(n => n.CreatedById == createdById.Value);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(n =>
                Contains(n.NewsTitle, keyword) ||
                Contains(n.Headline, keyword) ||
                Contains(n.NewsContent, keyword) ||
                Contains(_categories.GetById(n.CategoryId)?.CategoryName, keyword));
        }

        return query.Select(ToDisplay).ToList();
    }

    public IReadOnlyList<NewsArticleDisplay> Report(DateTime startDate, DateTime endDate)
    {
        return _news.GetAll()
            .Where(n => n.CreatedDate.Date >= startDate.Date && n.CreatedDate.Date <= endDate.Date)
            .OrderByDescending(n => n.CreatedDate)
            .Select(ToDisplay)
            .ToList();
    }

    public NewsArticleDisplay? GetDisplay(string id)
    {
        var article = _news.GetById(id);
        return article is null ? null : ToDisplay(article);
    }

    private NewsArticleDisplay ToDisplay(NewsArticle article)
    {
        return new NewsArticleDisplay
        {
            Article = article,
            CategoryName = _categories.GetById(article.CategoryId)?.CategoryName ?? "Unknown",
            AuthorName = _accounts.GetById(article.CreatedById)?.AccountName ?? "Administrator",
            Tags = _tags.GetAll().Where(t => article.TagIds.Contains(t.TagId)).ToList()
        };
    }

    private static bool Contains(string? value, string keyword) => value?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true;
}
