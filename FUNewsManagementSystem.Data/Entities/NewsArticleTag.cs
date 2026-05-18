namespace FUNewsManagementSystem.Data.Entities;

public sealed class NewsArticleTag
{
    public string NewsArticleId { get; set; } = string.Empty;

    public int TagId { get; set; }

    public NewsArticle? NewsArticle { get; set; }

    public Tag? Tag { get; set; }
}
