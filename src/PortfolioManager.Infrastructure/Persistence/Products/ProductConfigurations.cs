using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PortfolioManager.Domain.Products;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Persistence.Products;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("product");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id).ValueGeneratedNever();
        builder.Property(i => i.CreatedAt).HasColumnType("timestamptz");
        builder.Property(i => i.Version).IsRowVersion();
        builder.Property(i => i.UpdatedAt).HasColumnType("timestamptz");
        builder.Property(i => i.MinimumInvestmentAmount).HasDefaultValue(0);
        builder.Property(i => i.MinimumInvestmentAmount).HasDefaultValue(0);
        builder.Property(i => i.IsAvailable).HasDefaultValue(true);
        builder.Property(i => i.DueAt).HasColumnType("timestamptz");

        var parameters = new[]
{
            new object[] {
                new DateTime(2024, 01, 01).ToUniversalTime(),
                "XP Debêntures Incentivadas Hedge CP FIC FIM LP",
                new DateTime(2024, 01, 01).ToUniversalTime().AddYears(5),
                1000M,
                "manager@mail.com",
                Guid.Parse("8ba38709-9633-4a93-9824-d4ee22951c26")
            },
            [
                new DateTime(2024, 01, 01).ToUniversalTime(),
                "XP BNP Multi Asset A.I. - Alta Alavancada",
                new DateTime(2024, 01, 01).ToUniversalTime().AddYears(5),
                5000M,
                "manager@mail.com",
                Guid.Parse("6610e835-8a46-45dd-8b3d-2e3a64eaec6a")
            ]
        };

        DataSeeder<Product>.SeedData(builder, parameters);
    }
}