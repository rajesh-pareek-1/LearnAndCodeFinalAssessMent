using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Infrastructure.Data.Seed
{
    public static class NewsSyncNewsSeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var user1 = "68be1f19-76cc-4696-8b01-e534717afe68";
            var user2 = "7cf397ec-ca7c-4c1f-aea0-68184da919c7";

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Technology", Description = "Tech News" },
                new Category { Id = 2, Name = "Sports", Description = "Sports News" }
            );

            modelBuilder.Entity<Article>().HasData(
                new Article
                {
                    Id = 1,
                    Headline = "AI Breakthrough",
                    Description = "AI is transforming industries.",
                    Source = "TechCrunch",
                    Url = "https://example.com/ai",
                    CategoryId = 1,
                    AuthorName = "Jane",
                    ImageUrl = "https://example.com/img.jpg",
                    Language = "English",
                    PublishedDate = "2025-06-14"
                },
                new Article
                {
                    Id = 2,
                    Headline = "Olympics 2024 Highlights",
                    Description = "Day 1 recap of major events.",
                    Source = "ESPN",
                    Url = "https://example.com/olympics",
                    CategoryId = 2,
                    AuthorName = "Mike",
                    ImageUrl = "https://example.com/olympics.jpg",
                    Language = "English",
                    PublishedDate = "2024-07-25"
                }
            );

            modelBuilder.Entity<SavedArticle>().HasData(
                new SavedArticle { Id = 1, UserId = user1, ArticleId = 1 },
                new SavedArticle { Id = 2, UserId = user2, ArticleId = 2 }
            );

            modelBuilder.Entity<Notification>().HasData(
                new Notification
                {
                    Id = 1,
                    UserId = user1,
                    ArticleId = 1,
                    SentAt = new DateTime(2025, 06, 17, 10, 30, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 2,
                    UserId = user2,
                    ArticleId = 2,
                    SentAt = new DateTime(2025, 06, 18, 08, 15, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<NotificationConfiguration>().HasData(
                new NotificationConfiguration { Id = 1, UserId = user1, CategoryId = 1 },
                new NotificationConfiguration { Id = 2, UserId = user2, CategoryId = 2 }
            );

            modelBuilder.Entity<ServerDetail>().HasData(
                new ServerDetail
                {
                    Id = 1,
                    ServerName = "NewsAPI SyncNode",
                    LastAccess = new DateTime(2025, 06, 14, 10, 30, 0, DateTimeKind.Utc),
                    Status = "Active",
                    ApiKey = "4de532f9b8f941fb97ceee7df1ec2445",
                    BaseUrl = "https://newsapi.org/v2/everything"
                },
                new ServerDetail
                {
                    Id = 2,
                    ServerName = "TheNewsAPI SyncNode",
                    LastAccess = new DateTime(2025, 06, 14, 10, 30, 0, DateTimeKind.Utc),
                    Status = "Idle",
                    ApiKey = "Kjar4Jl0m6AvjigZQUFx8c0WuFLejmJsJ6CAXPyD",
                    BaseUrl = "https://api.thenewsapi.com/v1/news/top"
                }
            );

            modelBuilder.Entity<Keyword>().HasData(
                new Keyword { Id = 1, Name = "AI", UserId = user1 },
                new Keyword { Id = 2, Name = "Olympics", UserId = user2 }
            );

            modelBuilder.Entity<ArticleReport>()
       .HasOne(ar => ar.Article)
       .WithMany()
       .HasForeignKey(ar => ar.ArticleId)
       .OnDelete(DeleteBehavior.Restrict);  // Or just don't define any nav at all

            modelBuilder.Entity<ArticleReaction>()
            .HasOne(r => r.Article)
            .WithMany() // no navigation property on Article
            .HasForeignKey(r => r.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
