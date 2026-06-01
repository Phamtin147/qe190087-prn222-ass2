using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Data.Entities;

public sealed class Category
{
    public short CategoryId { get; set; }

    [Required, StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [Required, StringLength(250)]
    public string CategoryDescription { get; set; } = string.Empty;

    public short? ParentCategoryId { get; set; }

    public bool IsActive { get; set; } = true;

    public Category? ParentCategory { get; set; }

    public List<Category> InverseParentCategory { get; set; } = new();

    public List<NewsArticle> NewsArticles { get; set; } = new();
}
