using FUNewsManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Data.Repositories;

public sealed class TagRepository : IRepository<Tag, int>
{
    private readonly NewsManagementDbContext _context;

    public TagRepository(NewsManagementDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Tag> GetAll() => _context.Tags.AsNoTracking().OrderBy(t => t.TagName).ToList();

    public Tag? GetById(int id) => _context.Tags.AsNoTracking().SingleOrDefault(t => t.TagId == id);

    public void Add(Tag entity)
    {
        entity.TagId = (_context.Tags.Max(t => (int?)t.TagId) ?? 0) + 1;
        _context.Tags.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Tag entity)
    {
        var existing = _context.Tags.SingleOrDefault(t => t.TagId == entity.TagId) ?? throw new InvalidOperationException("Tag not found.");
        existing.TagName = entity.TagName;
        existing.Note = entity.Note;
        _context.SaveChanges();
    }

    public bool Delete(int id)
    {
        var existing = _context.Tags.SingleOrDefault(t => t.TagId == id);
        if (existing is null || _context.NewsTags.Any(n => n.TagId == id))
        {
            return false;
        }

        _context.Tags.Remove(existing);
        _context.SaveChanges();
        return true;
    }
}
