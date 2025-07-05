using Microsoft.EntityFrameworkCore;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Infrastructure.Data.Seed
{
    public static class NewsDataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedCategories(modelBuilder);
            SeedArticles(modelBuilder);
            SeedSavedArticles(modelBuilder);
            SeedNotifications(modelBuilder);
            SeedNotificationConfigurations(modelBuilder);
            SeedServerDetails(modelBuilder);
            SeedKeywords(modelBuilder);
            ConfigureRelationships(modelBuilder);
        }

        private static void SeedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Technology", Description = "Tech News" },
                new Category { Id = 2, Name = "Sports", Description = "Sports News" },
                new Category { Id = 3, Name = "Health", Description = "Health & Wellness" },
                new Category { Id = 4, Name = "Business", Description = "Business and Finance" },
                new Category { Id = 5, Name = "Entertainment", Description = "Movies, Music & Celebs" },
                new Category { Id = 6, Name = "Politics", Description = "Political News" },
                new Category { Id = 7, Name = "Science", Description = "Scientific Discoveries" },
                new Category { Id = 8, Name = "World", Description = "Global News" },
                new Category { Id = 9, Name = "Education", Description = "Education & Learning" },
                new Category { Id = 10, Name = "Environment", Description = "Environmental Issues" },
                new Category { Id = 11, Name = "Travel", Description = "Travel & Tourism" },
                new Category { Id = 12, Name = "Food", Description = "Food & Cooking" },
                new Category { Id = 13, Name = "Art", Description = "Art & Creativity" },
                new Category { Id = 14, Name = "Fashion", Description = "Fashion & Style" },
                new Category { Id = 15, Name = "Crime", Description = "Crime & Justice" },
                new Category { Id = 16, Name = "Opinion", Description = "Editorial Opinions" },
                new Category { Id = 17, Name = "Economy", Description = "Economics & Markets" },
                new Category { Id = 18, Name = "Automotive", Description = "Cars & Automobiles" },
                new Category { Id = 19, Name = "Religion", Description = "Religion & Spirituality" },
                new Category { Id = 20, Name = "Culture", Description = "Culture & Society" }
            );
        }

        private static void SeedArticles(ModelBuilder modelBuilder)
        {
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
        }

        private static void SeedSavedArticles(ModelBuilder modelBuilder)
        {
            const string user1Id = "68be1f19-76cc-4696-8b01-e534717afe68";
            const string user2Id = "7cf397ec-ca7c-4c1f-aea0-68184da919c7";

            modelBuilder.Entity<SavedArticle>().HasData(
                new SavedArticle { Id = 1, UserId = user1Id, ArticleId = 1 },
                new SavedArticle { Id = 2, UserId = user2Id, ArticleId = 2 }
            );
        }

        private static void SeedNotifications(ModelBuilder modelBuilder)
        {
            const string user1Id = "68be1f19-76cc-4696-8b01-e534717afe68";
            const string user2Id = "7cf397ec-ca7c-4c1f-aea0-68184da919c7";

            modelBuilder.Entity<Notification>().HasData(
                new Notification
                {
                    Id = 1,
                    UserId = user1Id,
                    ArticleId = 1,
                    SentAt = new DateTime(2025, 6, 17, 10, 30, 0, DateTimeKind.Utc)
                },
                new Notification
                {
                    Id = 2,
                    UserId = user2Id,
                    ArticleId = 2,
                    SentAt = new DateTime(2025, 6, 18, 8, 15, 0, DateTimeKind.Utc)
                }
            );
        }

        private static void SeedNotificationConfigurations(ModelBuilder modelBuilder)
        {
            const string user1Id = "68be1f19-76cc-4696-8b01-e534717afe68";
            const string user2Id = "7cf397ec-ca7c-4c1f-aea0-68184da919c7";

            modelBuilder.Entity<NotificationConfiguration>().HasData(
                new NotificationConfiguration { Id = 1, UserId = user1Id, CategoryId = 1 },
                new NotificationConfiguration { Id = 2, UserId = user2Id, CategoryId = 2 }
            );
        }

        private static void SeedServerDetails(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServerDetail>().HasData(
                new ServerDetail
                {
                    Id = 1,
                    ServerName = "NewsAPI SyncNode",
                    LastAccess = new DateTime(2025, 6, 14, 10, 30, 0, DateTimeKind.Utc),
                    Status = "Active",
                    ApiKey = "4de532f9b8f941fb97ceee7df1ec2445",
                    BaseUrl = "https://newsapi.org/v2/everything"
                },
                new ServerDetail
                {
                    Id = 2,
                    ServerName = "TheNewsAPI SyncNode",
                    LastAccess = new DateTime(2025, 6, 14, 10, 30, 0, DateTimeKind.Utc),
                    Status = "Idle",
                    ApiKey = "Kjar4Jl0m6AvjigZQUFx8c0WuFLejmJsJ6CAXPyD",
                    BaseUrl = "https://api.thenewsapi.com/v1/news/top"
                }
            );
        }

        private static void SeedKeywords(ModelBuilder modelBuilder)
        {
            const string user1Id = "68be1f19-76cc-4696-8b01-e534717afe68";
            const string user2Id = "7cf397ec-ca7c-4c1f-aea0-68184da919c7";

            modelBuilder.Entity<Keyword>().HasData(
                new Keyword { Id = 1, Name = "AI", UserId = user1Id },
                new Keyword { Id = 2, Name = "Olympics", UserId = user2Id }
            );
        }

        private static void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleReport>()
                .HasOne(ar => ar.Article)
                .WithMany()
                .HasForeignKey(ar => ar.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ArticleReaction>()
                .HasOne(r => r.Article)
                .WithMany()
                .HasForeignKey(r => r.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
