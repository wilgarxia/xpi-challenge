using Microsoft.EntityFrameworkCore;

using PortfolioManager.Domain.Products;
using PortfolioManager.Domain.Transactions;
using PortfolioManager.Domain.Users;

namespace PortfolioManager.Infrastructure.Persistence.Commom;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Manager> Managers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}