using FUNewsManagementSystem.Data.Entities;

namespace FUNewsManagementSystem.Data.Repositories;

public sealed class NewsManagementStore
{
    private static readonly Lazy<NewsManagementStore> LazyInstance = new(() => new NewsManagementStore());

    public static NewsManagementStore Instance => LazyInstance.Value;

    private NewsManagementStore()
    {
        Categories =
        [
            new Category { CategoryId = 1, CategoryName = "Academic news", CategoryDescription = "Research findings, faculty appointments and academic announcements.", ParentCategoryId = 1, IsActive = true },
            new Category { CategoryId = 2, CategoryName = "Student Affairs", CategoryDescription = "Student activities, events, clubs, organizations and sports.", ParentCategoryId = 2, IsActive = true },
            new Category { CategoryId = 3, CategoryName = "Campus Safety", CategoryDescription = "Incidents and safety measures implemented on campus.", ParentCategoryId = 3, IsActive = true },
            new Category { CategoryId = 4, CategoryName = "Alumni News", CategoryDescription = "Achievements and accomplishments of former students and alumni.", ParentCategoryId = 4, IsActive = true },
            new Category { CategoryId = 5, CategoryName = "Capstone Project News", CategoryDescription = "Reports created as part of academic or professional capstone projects.", ParentCategoryId = 5, IsActive = false }
        ];

        Accounts =
        [
            new SystemAccount { AccountId = 1, AccountName = "Staff Writer", AccountEmail = "staff@funews.org", AccountPassword = "123", AccountRole = AccountRole.Staff },
            new SystemAccount { AccountId = 2, AccountName = "Lecturer Reader", AccountEmail = "lecturer@funews.org", AccountPassword = "123", AccountRole = AccountRole.Lecturer }
        ];

        Tags =
        [
            new Tag { TagId = 1, TagName = "University", Note = "University wide news" },
            new Tag { TagId = 2, TagName = "Research", Note = "Academic research" },
            new Tag { TagId = 3, TagName = "Student", Note = "Student life" },
            new Tag { TagId = 4, TagName = "Safety", Note = "Campus safety" }
        ];

        NewsArticles =
        [
            new NewsArticle
            {
                NewsArticleId = "1",
                NewsTitle = "University FU Celebrates Success of Alumni in Various Fields",
                Headline = "University FU Celebrates Success of Alumni in Various Fields",
                CreatedDate = new DateTime(2024, 5, 5),
                NewsContent = "University FU commemorated the achievements of alumni who have excelled in multiple fields.",
                NewsSource = "FU News Desk",
                CategoryId = 4,
                NewsStatus = true,
                CreatedById = 1,
                TagIds = [1]
            },
            new NewsArticle
            {
                NewsArticleId = "2",
                NewsTitle = "Campus Safety Team Announces New Emergency Drill Schedule",
                Headline = "New campus safety drill schedule released",
                CreatedDate = new DateTime(2024, 6, 12),
                NewsContent = "The safety team will organize emergency drills across the campus during June.",
                NewsSource = "Campus Safety Office",
                CategoryId = 3,
                NewsStatus = true,
                CreatedById = 1,
                TagIds = [4]
            }
        ];
    }

    public List<Category> Categories { get; }
    public List<SystemAccount> Accounts { get; }
    public List<Tag> Tags { get; }
    public List<NewsArticle> NewsArticles { get; }

    public short NextCategoryId() => Categories.Count == 0 ? (short)1 : (short)(Categories.Max(c => c.CategoryId) + 1);
    public short NextAccountId() => Accounts.Count == 0 ? (short)1 : (short)(Accounts.Max(a => a.AccountId) + 1);
    public int NextTagId() => Tags.Count == 0 ? 1 : Tags.Max(t => t.TagId) + 1;
    public string NextNewsArticleId() => NewsArticles.Count == 0 ? "1" : (NewsArticles.Select(n => int.TryParse(n.NewsArticleId, out var value) ? value : 0).Max() + 1).ToString();
}
