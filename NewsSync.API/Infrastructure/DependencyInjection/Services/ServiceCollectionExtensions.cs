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
using NewsSync.API.Application.Services;
using NewsSync.API.Infrastructure.Configurations;

namespace NewsSync.API.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<NewsSyncAuthDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("AuthConnection")));

            services.AddDbContext<NewsSyncNewsDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("NewsConnection")));

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

            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IServerRepository, ServerRepository>();
            services.AddScoped<IArticleService, ArticleService>();

            services.AddScoped<ISavedArticleRepository, SavedArticleRepository>();
            services.AddScoped<ISavedArticleService, SavedArticleService>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IAdminService, AdminService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IArticleCategoryService, ArticleCategoryService>();

            services.AddScoped<IArticleReactionRepository, ArticleReactionRepository>();
            services.AddScoped<IArticleReactionService, ArticleReactionService>();

            services.Configure<SmtpSettings>(config.GetSection("Email:Smtp"));
            services.AddScoped<IUserNotificationService, EmailNotificationService>();

            services.AddHttpClient();
            services.AddSingleton<NewsApiOrgClientAdapter>();
            services.AddSingleton<TheNewsApiClientAdapter>();

            services.AddSingleton<INewsAdapter>(sp => sp.GetRequiredService<NewsApiOrgClientAdapter>());

            services.AddSingleton<Dictionary<string, INewsAdapter>>(sp => new()
            {
                ["NewsAPI SyncNode"] = sp.GetRequiredService<NewsApiOrgClientAdapter>(),
                ["TheNewsAPI SyncNode"] = sp.GetRequiredService<TheNewsApiClientAdapter>()
            });

            services.AddHostedService<NewsFetcherBackgroundService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles));

            return services;
        }
    }
}
