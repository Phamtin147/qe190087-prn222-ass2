using FUNewsManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Data.Repositories;

public sealed class CategoryRepository : IRepository<Category, short>
{
    private readonly NewsManagementDbContext _context;

    public CategoryRepository(NewsManagementDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Category> GetAll() => _context.Categories.AsNoTracking().OrderBy(c => c.CategoryName).ToList();

    public Category? GetById(short id) => _context.Categories.AsNoTracking().SingleOrDefault(c => c.CategoryId == id);

    public void Add(Category entity)
    {
        _context.Categories.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Category entity)
    {
        var existing = _context.Categories.SingleOrDefault(c => c.CategoryId == entity.CategoryId) ?? throw new InvalidOperationException("Category not found.");
        existing.CategoryName = entity.CategoryName;
        existing.CategoryDescription = entity.CategoryDescription;
        existing.ParentCategoryId = entity.ParentCategoryId;
        existing.IsActive = entity.IsActive;
        _context.SaveChanges();
    }

    public bool Delete(short id)
    {
        var existing = _context.Categories.SingleOrDefault(c => c.CategoryId == id);
        if (existing is null || _context.NewsArticles.Any(n => n.CategoryId == id))
        {
            return false;
        }

        _context.Categories.Remove(existing);
        _context.SaveChanges();
        return true;
    }
}
