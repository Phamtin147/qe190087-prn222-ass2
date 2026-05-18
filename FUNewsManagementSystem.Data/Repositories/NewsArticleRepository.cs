using FUNewsManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Data.Repositories;

public sealed class NewsArticleRepository : IRepository<NewsArticle, string>
{
    private readonly NewsManagementDbContext _context;

    public NewsArticleRepository(NewsManagementDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<NewsArticle> GetAll()
    {
        var articles = _context.NewsArticles.AsNoTracking().OrderByDescending(n => n.CreatedDate).ToList();
        LoadTagIds(articles);
        return articles;
    }

    public NewsArticle? GetById(string id)
    {
        var article = _context.NewsArticles.AsNoTracking().SingleOrDefault(n => n.NewsArticleId == id);
        if (article is not null)
        {
            LoadTagIds([article]);
        }

        return article;
    }

    public void Add(NewsArticle entity)
    {
        entity.NewsArticleId = NextNewsArticleId();
        entity.CreatedDate = entity.CreatedDate == default ? DateTime.Today : entity.CreatedDate;
        var tagIds = entity.TagIds.ToList();
        entity.TagIds = [];
        _context.NewsArticles.Add(entity);
        _context.SaveChanges();
        ReplaceTags(entity.NewsArticleId, tagIds);
    }

    public void Update(NewsArticle entity)
    {
        var existing = _context.NewsArticles.SingleOrDefault(n => n.NewsArticleId == entity.NewsArticleId) ?? throw new InvalidOperationException("News article not found.");
        existing.NewsTitle = entity.NewsTitle;
        existing.Headline = entity.Headline;
        existing.CreatedDate = entity.CreatedDate;
        existing.NewsContent = entity.NewsContent;
        existing.NewsSource = entity.NewsSource;
        existing.CategoryId = entity.CategoryId;
        existing.NewsStatus = entity.NewsStatus;
        existing.UpdatedById = entity.UpdatedById;
        existing.ModifiedDate = DateTime.Now;
        _context.SaveChanges();
        ReplaceTags(entity.NewsArticleId, entity.TagIds);
    }

    public bool Delete(string id)
    {
        var existing = _context.NewsArticles.SingleOrDefault(n => n.NewsArticleId == id);
        if (existing is null)
        {
            return false;
        }

        _context.Database.ExecuteSqlRaw("DELETE FROM NewsTag WHERE NewsArticleID = {0}", id);
        _context.NewsArticles.Remove(existing);
        _context.SaveChanges();
        return true;
    }

    private void LoadTagIds(List<NewsArticle> articles)
    {
        foreach (var article in articles)
        {
            article.TagIds = _context.NewsTags.AsNoTracking().Where(n => n.NewsArticleId == article.NewsArticleId).Select(n => n.TagId).ToList();
        }
    }

    private string NextNewsArticleId()
    {
        var ids = _context.NewsArticles.Select(n => n.NewsArticleId).ToList();
        var nextId = ids.Select(id => int.TryParse(id, out var value) ? value : 0).DefaultIfEmpty(0).Max() + 1;
        return nextId.ToString();
    }

    private void ReplaceTags(string articleId, IEnumerable<int> tagIds)
    {
        _context.NewsTags.RemoveRange(_context.NewsTags.Where(n => n.NewsArticleId == articleId));
        _context.NewsTags.AddRange(tagIds.Distinct().Select(tagId => new NewsArticleTag { NewsArticleId = articleId, TagId = tagId }));
        _context.SaveChanges();
    }
}
