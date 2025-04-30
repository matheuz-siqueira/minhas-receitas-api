using Microsoft.Extensions.Configuration;
using MinhasReceitasApp.Domain.Enums;

namespace MinhasReceitasApp.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("connection")!;
    }

    public static DatabaseType DatabaseType(this IConfiguration configuration)
    {
        var databaseType = configuration.GetConnectionString("databaseType");

        return (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType!);
    }

    public static bool IsUnitTestEnviroment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }
}
