using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using PortfolioManager.Domain.Common;
using PortfolioManager.Domain.UserAggregate;
using PortfolioManager.Infrastructure.Persistence.Commom;
using PortfolioManager.Infrastructure.Persistence.InvestmentAggregate;
using PortfolioManager.Infrastructure.Persistence.UserAggregate;
using PortfolioManager.Infrastructure.Security;

namespace PortfolioManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();

        NpgsqlDataSourceBuilder dataSourceBuilder = new(config.GetConnectionString("DefaultConnection"));
        NpgsqlDataSource dataSource = dataSourceBuilder.Build();

        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(dataSource)
            .UseSnakeCaseNamingConvention());

        services.AddStackExchangeRedisCache(o => o.Configuration = config.GetConnectionString("Cache"));

        services.AddOptions<JwtOptions>().Bind(config.GetSection("JwtSettings"));
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHashProvider, PasswordHashProvider>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IInvestmentRepository, InvestmentRepository>();

        return services;
    }
}