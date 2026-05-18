using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Data.Entities;

public sealed class SystemAccount
{
    public short AccountId { get; set; }

    [Required, StringLength(100)]
    public string AccountName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(70)]
    public string AccountEmail { get; set; } = string.Empty;

    public AccountRole AccountRole { get; set; }

    [Required, StringLength(70)]
    public string AccountPassword { get; set; } = string.Empty;

    public List<NewsArticle> NewsArticles { get; set; } = [];
}

public enum AccountRole
{
    Admin = 0,
    Staff = 1,
    Lecturer = 2
}
