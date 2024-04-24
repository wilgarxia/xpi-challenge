using System.Net.Mail;
using System.Net;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using PortfolioManager.Domain.Products;
using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Common;
using PortfolioManager.Infrastructure.Persistence.Commom;
using PortfolioManager.Infrastructure.Persistence.Products;
using PortfolioManager.Infrastructure.Persistence.Transactions;
using PortfolioManager.Infrastructure.Persistence.Users;
using PortfolioManager.Infrastructure.Security.CurrentUser;
using PortfolioManager.Infrastructure.Security.Jwt;
using PortfolioManager.Infrastructure.Security.PasswordHash;
using PortfolioManager.Infrastructure.Email;

namespace PortfolioManager.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddPersistence(configuration)
            .AddCache(configuration)
            .AddSecurity(configuration)
            .AddRepositories()
            .AddEmailNotifications(configuration);

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        NpgsqlDataSourceBuilder dataSourceBuilder = new(configuration.GetConnectionString("DefaultConnection"));
        NpgsqlDataSource dataSource = dataSourceBuilder.Build();

        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(dataSource)
            .UseSnakeCaseNamingConvention());

        return services;
    }

    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(o => o.Configuration = configuration.GetConnectionString("Cache"));

        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();
        services.AddSingleton<IPasswordHashProvider, PasswordHashProvider>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IPortfolioProductRepository, PortfolioProductRepository>();

        return services;
    }

    private static IServiceCollection AddEmailNotifications(this IServiceCollection services, IConfiguration configuration)
    {
        EmailSettings emailSettings = new();
        configuration.Bind(EmailSettings.Section, emailSettings);

        services
            .AddFluentEmail(emailSettings.DefaultFromEmail)
            .AddSmtpSender(new SmtpClient(emailSettings.SmtpSettings.Server)
            {
                Port = emailSettings.SmtpSettings.Port,
                Credentials = new NetworkCredential(
                    emailSettings.SmtpSettings.Username,
                    emailSettings.SmtpSettings.Password),
            });

        return services;
    }
}