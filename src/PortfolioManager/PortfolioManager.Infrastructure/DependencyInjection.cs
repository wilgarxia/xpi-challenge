using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using PortfolioManager.Domain.UserAggregate;
using PortfolioManager.Infrastructure.Persistence.Commom;
using PortfolioManager.Infrastructure.Persistence.UserAggregate;
using PortfolioManager.Infrastructure.Security;

namespace PortfolioManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        NpgsqlDataSourceBuilder dataSourceBuilder = new(config.GetConnectionString("DefaultConnection"));

        dataSourceBuilder.UseNodaTime();

        NpgsqlDataSource dataSource = dataSourceBuilder.Build();

        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(dataSource, n => n.UseNodaTime())
            .UseSnakeCaseNamingConvention());

        services.AddOptions<JwtOptions>().Bind(config.GetSection("JwtSettings"));
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHashProvider, PasswordHashProvider>();

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}