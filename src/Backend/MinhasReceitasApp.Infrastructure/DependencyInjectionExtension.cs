using System.Reflection;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using Microsoft.Azure.Amqp.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhasReceitasApp.Domain.Enums;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using MinhasReceitasApp.Domain.Repositories.User;
using MinhasReceitasApp.Domain.Security.Cryptography;
using MinhasReceitasApp.Domain.Security.Tokens;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Domain.Services.ServiceBus;
using MinhasReceitasApp.Domain.Services.Storage;
using MinhasReceitasApp.Infrastructure.DataAccess;
using MinhasReceitasApp.Infrastructure.DataAccess.Repositories;
using MinhasReceitasApp.Infrastructure.Extensions;
using MinhasReceitasApp.Infrastructure.Security.Cryptography;
using MinhasReceitasApp.Infrastructure.Security.Tokens.Access.Generator;
using MinhasReceitasApp.Infrastructure.Security.Tokens.Access.Validator;
using MinhasReceitasApp.Infrastructure.Services.LoggedUser;
using MinhasReceitasApp.Infrastructure.Services.ServiceBus;
using MinhasReceitasApp.Infrastructure.Services.Storage;

namespace MinhasReceitasApp.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddTokens(services, configuration);
        AddLoggedUser(services);
        AddPasswordEncripter(services);
        AddAzureStorage(services, configuration);
        AddQueue(services, configuration);
        if (configuration.IsUnitTestEnviroment())
            return;

        var databaseType = configuration.DatabaseType();
        if (databaseType == DatabaseType.MySql)
        {
            AddDbContext_MySql(services, configuration);
            AddFluentMigrator_MySql(services, configuration);
        }
    }

    private static void AddDbContext_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 41));
        services.AddDbContext<MinhasReceitasAppDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();
        services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
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

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();
    private static void AddPasswordEncripter(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, BCryptNet>();
    }

    private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

        if (connectionString.NotEmpty())
        {
            services.AddScoped<IBlobStorageService>(
                c => new AzureStorageService(new BlobServiceClient(connectionString)));
        }
    }

    private static void AddQueue(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount");

        if (string.IsNullOrWhiteSpace(connectionString))
            return;

        var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        });

        var deleteQueue = new DeleteUserQueue(client.CreateSender("user"));

        var DeleteUserProcessor = new DeleteUserProcessor(client.CreateProcessor("user", new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 1
        }));

        services.AddSingleton(DeleteUserProcessor);

        services.AddScoped<IDeleteUserQueue>(options => deleteQueue);
    }
}
