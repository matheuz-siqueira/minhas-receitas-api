using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MinhasReceitasApp.Domain.Enums;
using MySqlConnector;

namespace MinhasReceitasApp.Infrastructure.MIgrations;

public static class DatabaseMigration
{
    public static void Migrate(DatabaseType databaseType, string connectionString, IServiceProvider serviceProvider)
    {
        if(databaseType == DatabaseType.MySql)
            EnsureDatabaseCreated_MySql(connectionString); 

        MigrationDataBase(serviceProvider); 
    }

    private static void EnsureDatabaseCreated_MySql(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString); 
        var databaseName = connectionStringBuilder.Database; 

        connectionStringBuilder.Remove("Database"); 
        using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);

        var parameters = new DynamicParameters(); 
        parameters.Add("name", databaseName);
        var records = dbConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", parameters); 

        if(!records.Any())
            dbConnection.Execute($"CREATE DATABASE {databaseName}");
    }

    private static void MigrationDataBase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
    }
}
