using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PortfolioManager.Domain.Users;

namespace PortfolioManager.Infrastructure.Persistence.Users;

internal class PortfolioProductsConfiguration : IEntityTypeConfiguration<PortfolioProduct>
{
    public void Configure(EntityTypeBuilder<PortfolioProduct> builder)
    {
        builder.ToTable("portfolio_product");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedNever();
        builder.Property(u => u.Version).IsRowVersion();
        builder.Property(u => u.CreatedAt).HasColumnType("timestamptz");
        builder.Property(u => u.UpdatedAt).HasColumnType("timestamptz");

        builder.HasOne(x => x.User).WithMany(x => x.PortfolioProducts);
        builder.HasOne(x => x.Product);
    }
}
