using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PortfolioManager.Domain.Users;

namespace PortfolioManager.Infrastructure.Persistence.Transactions;

internal class TransactionConfigurations : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transaction");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        builder.Property(x => x.Version).IsRowVersion();
        builder.HasOne(x => x.User);
        builder.HasOne(x => x.Product);

        builder.HasIndex(x => x.UserId);
    }
}