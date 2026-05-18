using FUNewsManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Data.Repositories;

public sealed class AccountRepository : IRepository<SystemAccount, short>
{
    private readonly NewsManagementDbContext _context;

    public AccountRepository(NewsManagementDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<SystemAccount> GetAll() => _context.SystemAccounts.AsNoTracking().OrderBy(a => a.AccountName).ToList();

    public SystemAccount? GetById(short id) => _context.SystemAccounts.AsNoTracking().SingleOrDefault(a => a.AccountId == id);

    public SystemAccount? GetByEmail(string email) => _context.SystemAccounts.AsNoTracking().SingleOrDefault(a => a.AccountEmail == email);

    public void Add(SystemAccount entity)
    {
        entity.AccountId = (short)((_context.SystemAccounts.Max(a => (short?)a.AccountId) ?? 0) + 1);
        _context.SystemAccounts.Add(entity);
        _context.SaveChanges();
    }

    public void Update(SystemAccount entity)
    {
        var existing = _context.SystemAccounts.SingleOrDefault(a => a.AccountId == entity.AccountId) ?? throw new InvalidOperationException("Account not found.");
        existing.AccountName = entity.AccountName;
        existing.AccountEmail = entity.AccountEmail;
        existing.AccountRole = entity.AccountRole;
        existing.AccountPassword = entity.AccountPassword;
        _context.SaveChanges();
    }

    public bool Delete(short id)
    {
        var existing = _context.SystemAccounts.SingleOrDefault(a => a.AccountId == id);
        if (existing is null || _context.NewsArticles.Any(n => n.CreatedById == id || n.UpdatedById == id))
        {
            return false;
        }

        _context.SystemAccounts.Remove(existing);
        _context.SaveChanges();
        return true;
    }
}
