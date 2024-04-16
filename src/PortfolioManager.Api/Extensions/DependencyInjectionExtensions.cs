using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using PortfolioManager.Api.Extensions;
using PortfolioManager.Api.Identity;

namespace PortfolioManager.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddAuthentication(config)
            .AddAuthorization();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        });

        return services;
    }

    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(
                PolicyConfiguration.AdminUserPolicyName,
                p => p.RequireClaim(PolicyConfiguration.AdminUserClaimName, "true"));

        return services;
    }
}