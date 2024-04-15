using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PortfolioManager.Domain.UserAggregate;
using PortfolioManager.Infrastructure.Persistence.Commom;
using PortfolioManager.Infrastructure.Persistence.UserAggregate;
using PortfolioManager.Infrastructure.Security;

namespace PortfolioManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"))
                       .UseSnakeCaseNamingConvention());

        services.AddOptions<JwtOptions>().Bind(config.GetSection("JwtSettings"));
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}