using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PortfolioManager.Domain.InvestmentAggregate;

namespace PortfolioManager.Infrastructure.Persistence.InvestmentAggregate;

public class InvestmentConfigurations : IEntityTypeConfiguration<Investment>
{
    public void Configure(EntityTypeBuilder<Investment> builder)
    {
        builder.ToTable("investment");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id).ValueGeneratedNever();
        builder.Property(i => i.MinimumInvestmentAmount).HasDefaultValue(0);
        builder.Property(i => i.MinimumInvestmentAmount).HasDefaultValue(0);
        builder.Property(i => i.IsAvailable).HasDefaultValue(true);
        builder.Property(i => i.CreatedAt).HasColumnType("timestamp");
        builder.Property(i => i.UpdatedAt).HasColumnType("timestamp");
        builder.Property(i => i.DueAt).HasColumnType("timestamp");
        builder.HasOne(i => i.Manager).WithMany();

        builder.HasData(new Investment()
        {
            Id = Guid.Parse("8ba38709-9633-4a93-9824-d4ee22951c26"),
            CreatedAt = new DateTime(2024, 1, 1),
            UpdatedAt = null,
            Description = "XP Debêntures Incentivadas Hedge CP FIC FIM LP",
            DueAt = new DateTime(2024, 1, 1).AddYears(5),
            IsAvailable = true,
            UserId = Guid.Parse("aacbda5a-2add-469e-85b9-d14dff2eb38b"),
            MinimumInvestmentAmount = 1000,
            Risk = Risk.Medium,
            Type = InvestmentType.Bond
        });

        builder.HasData(new Investment()
        {
            Id = Guid.Parse("6610e835-8a46-45dd-8b3d-2e3a64eaec6a"),
            CreatedAt = new DateTime(2024, 1, 1),
            UpdatedAt = null,
            Description = "XP BNP Multi Asset A.I. - Alta Alavancada",
            DueAt = new DateTime(2024, 1, 1).AddYears(5),
            IsAvailable = true,
            UserId = Guid.Parse("aacbda5a-2add-469e-85b9-d14dff2eb38b"),
            MinimumInvestmentAmount = 5000,
            Risk = Risk.High,
            Type = InvestmentType.Fund
        });
    }
}