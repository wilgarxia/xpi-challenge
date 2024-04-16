using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Extensions;

public static class MigrationsExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();
    }
}