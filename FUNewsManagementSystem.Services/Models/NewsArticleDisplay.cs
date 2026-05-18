using FUNewsManagementSystem.Data.Entities;

namespace FUNewsManagementSystem.Services.Models;

public sealed class NewsArticleDisplay
{
    public required NewsArticle Article { get; init; }
    public required string CategoryName { get; init; }
    public required string AuthorName { get; init; }
    public required IReadOnlyList<Tag> Tags { get; init; }
}
