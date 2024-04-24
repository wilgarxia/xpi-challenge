using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using PortfolioManager.Application.Contracts;
using PortfolioManager.Application.Interfaces;
using PortfolioManager.Application.Services;

namespace PortfolioManager.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddServices();

        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<IPortfolioService, PortfolioService>();

        return services;
    }
}