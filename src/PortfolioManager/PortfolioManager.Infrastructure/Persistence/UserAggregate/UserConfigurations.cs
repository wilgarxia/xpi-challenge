using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PortfolioManager.Domain.UserAggregate;

namespace PortfolioManager.Infrastructure.Persistence.UserAggregate;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedNever();
        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.Username).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.IsAdmin).IsRequired().HasDefaultValue(false);

        builder.HasIndex("Username").IsUnique();

        builder.HasData(User.Create("admin", "123456", true));
        builder.HasData(User.Create("user1", "123456", false));
    }
}