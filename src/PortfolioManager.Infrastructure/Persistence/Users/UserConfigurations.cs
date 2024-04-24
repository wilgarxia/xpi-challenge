using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PortfolioManager.Domain.Users;
using PortfolioManager.Infrastructure.Persistence.Commom;

namespace PortfolioManager.Infrastructure.Persistence.Users;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");  

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedNever();
        builder.Property(u => u.IsAdmin).HasDefaultValue(false);
        builder.Property(u => u.Version).IsRowVersion();
        builder.Property(u => u.CreatedAt).HasColumnType("timestamptz");

        builder.HasIndex("Email").IsUnique();

        builder.HasMany(x => x.PortfolioProducts).WithOne(x => x.User);
        builder.HasMany(x => x.Transactions).WithOne(x => x.User);

        var parameters = new[]
        {
            new object[] {
                new DateTime(2024, 01, 01).ToUniversalTime(), 
                "John", 
                "Doe", 
                "john@example.com", 
                BCrypt.Net.BCrypt.HashPassword("hashedpassword1"), 
                true, 
                Guid.Parse("aacbda5a-2add-469e-85b9-d14dff2eb38b") 
            },
            [
                new DateTime(2024, 01, 01).ToUniversalTime(), 
                "Jane", 
                "Smith", 
                "jane@example.com", 
                BCrypt.Net.BCrypt.HashPassword("hashedpassword2"), 
                false, 
                Guid.Parse("0d706086-6829-45bc-ad95-b8b0d942aa84")
            ],
            [
                new DateTime(2024, 01, 01).ToUniversalTime(), 
                "Alice", 
                "Johnson", 
                "alice@example.com", 
                BCrypt.Net.BCrypt.HashPassword("hashedpassword3"), 
                true, 
                Guid.Parse("8d4e4aa3-0bdf-44d3-ba6a-e3383eacebc7")
            ]
        };

        DataSeeder<User>.SeedData(builder, parameters);
    }
}