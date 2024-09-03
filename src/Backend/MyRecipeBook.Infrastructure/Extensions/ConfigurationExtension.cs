using Microsoft.Extensions.Configuration;
using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static DatabaseType DatabaseType(this IConfiguration configuration)
        {
            var databaseType = configuration.GetConnectionString("DatabaseType");

            return (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType!);
        }

        public static string ConnectionString(this IConfiguration configuration)
        {
            var dataseType = configuration.DatabaseType();

            if (dataseType == Domain.Enums.DatabaseType.SqlServer)
                return configuration.GetConnectionString("connectionSQLServer")!;
            else
                return configuration.GetConnectionString("connectionOtherDB")!;
        }
    }
}
