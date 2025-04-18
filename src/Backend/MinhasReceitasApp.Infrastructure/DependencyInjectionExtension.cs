using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhasReceitasApp.Domain.Enums;
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.User;
using MinhasReceitasApp.Infrastructure.DataAccess;
using MinhasReceitasApp.Infrastructure.DataAccess.Repositories;

namespace MinhasReceitasApp.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseType = configuration.GetConnectionString("databaseType");

        var databaseTypeEnum = (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType!); 

        if(databaseTypeEnum == DatabaseType.MySql)
            AddDbContext_MySql(services, configuration); 
        
        AddRepositories(services); 
    }   

    private static void AddDbContext_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("connection"); 
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
    
}
