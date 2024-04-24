using System.Reflection;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PortfolioManager.Infrastructure.Persistence.Commom;

public class DataSeeder<T> where T : class
{
    private const string DefaultSeedCreateMethod = "CreateForDataSeed";

    public static void SeedData(EntityTypeBuilder<T> builder, object[][] parameters)
    {
        Type userType = typeof(T);

        MethodInfo? createForDataSeedMethod = userType.GetMethod(
            DefaultSeedCreateMethod,
            BindingFlags.NonPublic | BindingFlags.Static);

        if (createForDataSeedMethod == null)
        {
            Console.WriteLine("Method not found.");
            return;
        }

        foreach (var item in parameters)
        {
            T entity = (T)createForDataSeedMethod.Invoke(null, item)!;
            builder.HasData(entity);
        }
    }
}