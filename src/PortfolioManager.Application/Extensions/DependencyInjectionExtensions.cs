using Microsoft.Extensions.DependencyInjection;

using PortfolioManager.Application.Services;

namespace PortfolioManager.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IInvestmentService, InvestmentService>();

        return services;
    }
}