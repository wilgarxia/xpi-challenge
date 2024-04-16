using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain.InvestmentAggregate;
using PortfolioManager.Domain.UserAggregate;

namespace PortfolioManager.Infrastructure.Persistence.Commom;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Investment> Investments { get; set; }
}