using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain;

namespace PortfolioManager.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
        .UseNpgsql()
        .UseSnakeCaseNamingConvention();

    public DbSet<User> User { get; set; }
}