using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PortfolioManager.Domain.Products;

namespace PortfolioManager.Infrastructure.Persistence.Products;

public class ManagerConfigurations : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.ToTable("manager");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id).ValueGeneratedNever();
        builder.Property(i => i.CreatedAt).HasColumnType("timestamp");
        builder.Property(i => i.Version).IsRowVersion();

        builder.HasData(new Manager()
        {
            Id = Guid.Parse("700b3357-29a6-437c-ba7e-e31abae30049"),
            CreatedAt = new DateTime(2024, 1, 1),
            FirstName = "Gandalf",
            Lastname = "The Grey",
            Email = "gandalf@gmail.com"
        });
    }
}