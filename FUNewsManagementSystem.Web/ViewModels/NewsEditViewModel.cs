using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Web.ViewModels;

public sealed class NewsEditViewModel
{
    public string? NewsArticleId { get; set; }

    [StringLength(400)]
    public string? NewsTitle { get; set; }

    [Required, StringLength(150)]
    public string Headline { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime CreatedDate { get; set; } = DateTime.Today;

    [StringLength(4000)]
    public string? NewsContent { get; set; }

    [StringLength(400)]
    public string? NewsSource { get; set; }

    [Required]
    public short CategoryId { get; set; }

    public bool NewsStatus { get; set; } = true;

    public List<int> TagIds { get; set; } = [];
}
