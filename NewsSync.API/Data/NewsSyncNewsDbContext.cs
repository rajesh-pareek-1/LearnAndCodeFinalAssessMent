using Microsoft.EntityFrameworkCore;
using NewsSync.API.Models.Domain;

namespace NewsSync.API.Data
{
    public class NewsSyncNewsDbContext : DbContext
    {
        public NewsSyncNewsDbContext(DbContextOptions<NewsSyncNewsDbContext> options) : base(options) { }
        public DbSet<Article> Articles { get; set; }
        public DbSet<SavedArticle> SavedArticles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationConfiguration> NotificationConfigurations { get; set; }
        public DbSet<ServerDetail> ServerDetails { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example user IDs
            var user1 = "68be1f19-76cc-4696-8b01-e534717afe68";
            var user2 = "7cf397ec-ca7c-4c1f-aea0-68184da919c7";

            // Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Technology", Description = "Tech News" },
                new Category { Id = 2, Name = "Sports", Description = "Sports News" }
            );

            // Articles
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

            // SavedArticles
            modelBuilder.Entity<SavedArticle>().HasData(
                new SavedArticle { Id = 1, UserId = user1, ArticleId = 1 },
                new SavedArticle { Id = 2, UserId = user2, ArticleId = 2 }
            );

            // Notifications
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

            // NotificationConfigurations
            modelBuilder.Entity<NotificationConfiguration>().HasData(
                new NotificationConfiguration { Id = 1, UserId = user1, CategoryId = 1 },
                new NotificationConfiguration { Id = 2, UserId = user2, CategoryId = 2 }
            );

            // ServerDetails
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


            // Keywords
            modelBuilder.Entity<Keyword>().HasData(
                new Keyword { Id = 1, Name = "AI", UserId = user1 },
                new Keyword { Id = 2, Name = "Olympics", UserId = user2 }
            );
        }
    }
}