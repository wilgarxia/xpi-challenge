using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PortfolioManager.Domain.Products;

namespace PortfolioManager.Infrastructure.Persistence.Products;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("product");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id).ValueGeneratedNever();
        builder.Property(i => i.CreatedAt).HasColumnType("timestamp");
        builder.Property(i => i.Version).IsRowVersion();
        builder.Property(i => i.UpdatedAt).HasColumnType("timestamp");
        builder.Property(i => i.MinimumInvestmentAmount).HasDefaultValue(0);
        builder.Property(i => i.MinimumInvestmentAmount).HasDefaultValue(0);
        builder.Property(i => i.IsAvailable).HasDefaultValue(true);
        builder.Property(i => i.DueAt).HasColumnType("timestamp");

        builder.HasOne(x => x.Manager).WithMany(x => x.Products);
        builder.HasMany(x => x.Users).WithMany(x => x.Products);

        builder.HasData(new Product()
        {
            Id = Guid.Parse("8ba38709-9633-4a93-9824-d4ee22951c26"),
            CreatedAt = new DateTime(2024, 1, 1),
            UpdatedAt = null,
            Description = "XP Debêntures Incentivadas Hedge CP FIC FIM LP",
            DueAt = new DateTime(2024, 1, 1).AddYears(5),
            IsAvailable = true,
            MinimumInvestmentAmount = 1000,
            ManagerId = Guid.Parse("700b3357-29a6-437c-ba7e-e31abae30049")
        });

        builder.HasData(new Product()
        {
            Id = Guid.Parse("6610e835-8a46-45dd-8b3d-2e3a64eaec6a"),
            CreatedAt = new DateTime(2024, 1, 1),
            UpdatedAt = null,
            Description = "XP BNP Multi Asset A.I. - Alta Alavancada",
            DueAt = new DateTime(2024, 1, 1).AddYears(5),
            IsAvailable = true,
            MinimumInvestmentAmount = 5000,
            ManagerId = Guid.Parse("700b3357-29a6-437c-ba7e-e31abae30049")
        });
    }
}