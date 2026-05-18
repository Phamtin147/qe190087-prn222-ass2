using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Data.Entities;

public sealed class Tag
{
    public int TagId { get; set; }

    [Required, StringLength(50)]
    public string TagName { get; set; } = string.Empty;

    [StringLength(400)]
    public string? Note { get; set; }

    public List<NewsArticleTag> NewsTags { get; set; } = [];
}
