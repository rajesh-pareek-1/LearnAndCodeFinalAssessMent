using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsSync.API.Domain.Entities;
using NewsSync.API.Application.Interfaces.Repositories;
using NewsSync.API.Application.Interfaces.Services;
using NewsSync.API.Infrastructure.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NewsSync.API.Infrastructure.Data;
using NewsSync.API.Infrastructure.Repositories;

namespace NewsSync.API.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // 🔐 DB Contexts
            services.AddDbContext<NewsSyncAuthDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("AuthConnection")));

            services.AddDbContext<NewsSyncNewsDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("NewsConnection")));

            // 🔐 Identity
            services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>("NewsSync")
                .AddEntityFrameworkStores<NewsSyncAuthDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            // 🔑 Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                    };
                });

            // 🧠 Services & Repositories
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleService, ArticleService>();

            services.AddScoped<ISavedArticleRepository, SavedArticleRepository>();
            services.AddScoped<ISavedArticleService, SavedArticleService>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdminService, AdminService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IArticleCategoryService, ArticleCategoryService>();

            services.AddScoped<IArticleReactionRepository, ArticleReactionRepository>();
            services.AddScoped<IArticleReactionService, ArticleReactionService>();

            services.AddScoped<IUserNotificationService, EmailNotificationService>();

            // 🧲 External Adapters
            services.AddHttpClient();
            services.AddSingleton<NewsApiOrgAdapter>();
            services.AddSingleton<TheNewsApiAdapter>();

            services.AddSingleton<INewsAdapter>(sp => sp.GetRequiredService<NewsApiOrgAdapter>());

            services.AddSingleton<Dictionary<string, INewsAdapter>>(sp => new()
            {
                ["NewsAPI SyncNode"] = sp.GetRequiredService<NewsApiOrgAdapter>(),
                ["TheNewsAPI SyncNode"] = sp.GetRequiredService<TheNewsApiAdapter>()
            });

            services.AddHostedService<NewsFetcherBackgroundService>();

            // 🔁 AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfiles));

            return services;
        }
    }
}
