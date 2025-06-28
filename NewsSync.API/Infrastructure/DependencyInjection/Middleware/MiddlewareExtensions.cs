using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using NewsSync.API.Middleware;

namespace NewsSync.API.Infrastructure.DependencyInjection
{
    public static class MiddlewareExtensions
    {
        public static WebApplication UseCustomMiddlewares(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
