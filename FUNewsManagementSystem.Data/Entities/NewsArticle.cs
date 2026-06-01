using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Data.Entities;

public sealed class NewsArticle
{
    [Required, StringLength(20)]
    public string NewsArticleId { get; set; } = string.Empty;

    [StringLength(400)]
    public string? NewsTitle { get; set; }

    [Required, StringLength(150)]
    public string Headline { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.Today;

    [StringLength(4000)]
    public string? NewsContent { get; set; }

    [StringLength(400)]
    public string? NewsSource { get; set; }

    [Required]
    public short CategoryId { get; set; }

    public bool NewsStatus { get; set; } = true;

    public short CreatedById { get; set; }

    public short? UpdatedById { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public List<int> TagIds { get; set; } = new();

    public Category? Category { get; set; }

    public SystemAccount? CreatedBy { get; set; }

    public List<NewsArticleTag> NewsTags { get; set; } = new();
}
