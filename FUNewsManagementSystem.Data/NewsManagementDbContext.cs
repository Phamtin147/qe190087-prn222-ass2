using FUNewsManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Data;

public sealed class NewsManagementDbContext : DbContext
{
    public NewsManagementDbContext(DbContextOptions<NewsManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<NewsArticle> NewsArticles => Set<NewsArticle>();

    public DbSet<NewsArticleTag> NewsTags => Set<NewsArticleTag>();

    public DbSet<SystemAccount> SystemAccounts => Set<SystemAccount>();

    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");
            entity.HasKey(e => e.CategoryId);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.CategoryDescription).HasColumnName("CategoryDesciption").HasMaxLength(250).IsRequired();
            entity.Property(e => e.ParentCategoryId).HasColumnName("ParentCategoryID");
            entity.Property(e => e.IsActive);

            entity.HasOne(e => e.ParentCategory)
                .WithMany(e => e.InverseParentCategory)
                .HasForeignKey(e => e.ParentCategoryId)
                .HasConstraintName("FK_Category_Category");
        });

        modelBuilder.Entity<NewsArticle>(entity =>
        {
            entity.ToTable("NewsArticle");
            entity.HasKey(e => e.NewsArticleId);
            entity.Property(e => e.NewsArticleId).HasColumnName("NewsArticleID").HasMaxLength(20);
            entity.Property(e => e.NewsTitle).HasMaxLength(400);
            entity.Property(e => e.Headline).HasMaxLength(150).IsRequired();
            entity.Property(e => e.NewsContent).HasMaxLength(4000);
            entity.Property(e => e.NewsSource).HasMaxLength(400);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");
            entity.Property(e => e.UpdatedById).HasColumnName("UpdatedByID");
            entity.Ignore(e => e.TagIds);

            entity.HasOne(e => e.Category)
                .WithMany(e => e.NewsArticles)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsArticle_Category");

            entity.HasOne(e => e.CreatedBy)
                .WithMany(e => e.NewsArticles)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsArticle_SystemAccount");
        });

        modelBuilder.Entity<NewsArticleTag>(entity =>
        {
            entity.ToTable("NewsTag");
            entity.HasKey(e => new { e.NewsArticleId, e.TagId });
            entity.Property(e => e.NewsArticleId).HasColumnName("NewsArticleID").HasMaxLength(20);
            entity.Property(e => e.TagId).HasColumnName("TagID");

            entity.HasOne(e => e.NewsArticle)
                .WithMany(e => e.NewsTags)
                .HasForeignKey(e => e.NewsArticleId)
                .HasConstraintName("FK_NewsTag_NewsArticle");

            entity.HasOne(e => e.Tag)
                .WithMany(e => e.NewsTags)
                .HasForeignKey(e => e.TagId)
                .HasConstraintName("FK_NewsTag_Tag");
        });

        modelBuilder.Entity<SystemAccount>(entity =>
        {
            entity.ToTable("SystemAccount");
            entity.HasKey(e => e.AccountId);
            entity.Property(e => e.AccountId).HasColumnName("AccountID").ValueGeneratedNever();
            entity.Property(e => e.AccountName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.AccountEmail).HasMaxLength(70).IsRequired();
            entity.Property(e => e.AccountPassword).HasMaxLength(70).IsRequired();
            entity.Property(e => e.AccountRole).HasConversion<int>();
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag");
            entity.HasKey(e => e.TagId);
            entity.Property(e => e.TagId).HasColumnName("TagID").ValueGeneratedNever();
            entity.Property(e => e.TagName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Note).HasMaxLength(400);
        });
    }
}
