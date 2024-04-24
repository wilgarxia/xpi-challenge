using System.Reflection;
using System.Text;

using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using PortfolioManager.Api.BackgroundServices;
using PortfolioManager.Api.Extensions;
using PortfolioManager.Infrastructure.Security.AuthorizationPolicies;

namespace PortfolioManager.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddEndpointsApiExplorer()
            .AddProblemDetails(o => o.CustomizeProblemDetails = c => 
                c.ProblemDetails.Extensions.Add("instance", $"{c.HttpContext.Request.Method} {c.HttpContext.Request.Path}"))
            .AddAuthentication(config)
            .AddAuthorization()
            .AddSwagger(config);

        services.AddHostedService<ProductDueDateCheckService>();

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Secret"]!)),
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
                AdminPolicyConfiguration.AdminUserPolicyName,
                p => p.RequireClaim(AdminPolicyConfiguration.AdminUserClaimName, "true"));

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = configuration["SwaggerConfig:Version"],
                Title = configuration["SwaggerConfig:Title"],
                Description = configuration["SwaggerConfig:Description"],
                Contact = new OpenApiContact
                {
                    Name = configuration["SwaggerConfig:ContactName"],
                    Email = configuration["SwaggerConfig:ContactEmail"],
                    Url = new Uri(configuration["SwaggerConfig:ContactUrl"]!)
                },
                License = new OpenApiLicense
                {
                    Name = configuration["SwaggerConfig:LicenseName"],
                    Url = new Uri(configuration["SwaggerConfig:LicenseUrl"]!)
                }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\nExample: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,  
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        services.AddFluentValidationRulesToSwagger();

        return services;
    }
}