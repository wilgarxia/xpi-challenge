using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PortfolioManager.Infrastructure.Persistence.Commom;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        string connectionString = args.Length < 1 ?
            "Host=localhost;Port=5432;Database=mydb;Username=myuser;Password=mypass" :
            args[0];

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();

        return new AppDbContext(optionsBuilder.Options);
    }
}