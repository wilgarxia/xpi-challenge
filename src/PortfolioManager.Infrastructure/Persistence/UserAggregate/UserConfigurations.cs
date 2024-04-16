using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PortfolioManager.Domain.UserAggregate;

namespace PortfolioManager.Infrastructure.Persistence.UserAggregate;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");  

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedNever();
        builder.Property(u => u.IsAdmin).HasDefaultValue(false);
        builder.Property(u => u.CreatedAt).HasColumnType("timestamp");

        builder.HasIndex("Username").IsUnique();

        builder.HasData(new User()
        {
            Id = Guid.Parse("aacbda5a-2add-469e-85b9-d14dff2eb38b"),
            CreatedAt = new DateTime(2024, 1, 1),
            Username = "admin",
            PasswordHash = "$2a$11$xfo0ikCN.paTU56KA3MR5ekr52..ps1wE2BiMPabUv2rnpQJSlyXK", // mySuperSecretPassword
            IsAdmin = true
        });
        builder.HasData(new User()
        {
            Id = Guid.Parse("0d706086-6829-45bc-ad95-b8b0d942aa84"),
            CreatedAt = new DateTime(2024, 1, 1),
            Username = "my-user",
            PasswordHash = "$2a$11$Ytj6xRsTIEVcvti/PhFeUOcpKZ3O53Yp.5e74xka0lXb3/FhJdvwu", // mySecretPassword
            IsAdmin = false
        });
    }
}