using Microsoft.Extensions.DependencyInjection;

using PortfolioManager.Application.Services;

namespace PortfolioManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}