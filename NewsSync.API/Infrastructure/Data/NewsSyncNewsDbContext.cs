using Microsoft.EntityFrameworkCore;
using NewsSync.API.Infrastructure.Data.Seed;
using NewsSync.API.Domain.Entities;

namespace NewsSync.API.Infrastructure.Data
{
    public class NewsSyncNewsDbContext : DbContext
    {
        public NewsSyncNewsDbContext(DbContextOptions<NewsSyncNewsDbContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SavedArticle> SavedArticles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationConfiguration> NotificationConfigurations { get; set; }
        public DbSet<ServerDetail> ServerDetails { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<ArticleReport> ArticleReports { get; set; }
        public DbSet<ArticleReaction> ArticleReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplySeedData(modelBuilder);
        }

        private static void ApplySeedData(ModelBuilder modelBuilder)
        {
            NewsDataSeeder.Seed(modelBuilder);
        }
    }
}
