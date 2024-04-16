using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using Npgsql;

namespace PortfolioManager.Infrastructure.Persistence.Commom;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        string connectionString = args.Length < 1 ?
            "Host=localhost;Port=5432;Database=mydb;Username=myuser;Password=mypass" :
            args[0];

        NpgsqlDataSourceBuilder dataSourceBuilder = new(connectionString);
        NpgsqlDataSource dataSource = dataSourceBuilder.Build();
        DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();

        optionsBuilder
            .UseNpgsql(dataSource)
            .UseSnakeCaseNamingConvention();

        return new AppDbContext(optionsBuilder.Options);
    }
}