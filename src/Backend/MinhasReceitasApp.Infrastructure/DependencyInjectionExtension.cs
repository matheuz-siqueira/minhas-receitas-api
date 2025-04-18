using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhasReceitasApp.Domain.Enums;
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.User;
using MinhasReceitasApp.Infrastructure.DataAccess;
using MinhasReceitasApp.Infrastructure.DataAccess.Repositories;
using MinhasReceitasApp.Infrastructure.Extensions;

namespace MinhasReceitasApp.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
       var databaseType = configuration.DatabaseType(); 
        if(databaseType == DatabaseType.MySql)
        {
            AddDbContext_MySql(services, configuration); 
            AddFluentMigrator_MySql(services, configuration);
        }
        AddRepositories(services); 
    }   

    private static void AddDbContext_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        var serverVersion = new MySqlServerVersion(new Version(8,0,41)); 
        services.AddDbContext<MinhasReceitasAppDbContext>(dbContextOptions => 
        {
            dbContextOptions.UseMySql(connectionString, serverVersion); 
        }); 
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();  
        services.AddScoped<IUnityOfWork, UnityOfWork>();
    }

    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
    { 
        var connectionString = configuration.ConnectionString();
        services.AddFluentMigratorCore().ConfigureRunner(options => 
        {
            options
                .AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("MinhasReceitasApp.Infrastructure")).For.All(); 
        }); 
    }
    
}
