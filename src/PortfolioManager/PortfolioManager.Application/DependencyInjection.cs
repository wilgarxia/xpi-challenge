using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PortfolioManager.Application.Services;
using PortfolioManager.Infrastructure;

namespace PortfolioManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();

        services.AddInfrastructure(config);

        return services;
    }
}