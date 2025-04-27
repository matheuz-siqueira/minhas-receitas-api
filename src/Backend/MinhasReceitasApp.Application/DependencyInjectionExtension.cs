using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhasReceitasApp.Application.Services.AutoMapper;
using MinhasReceitasApp.Application.Services.Cryptography;
using MinhasReceitasApp.Application.UseCases.Login.DoLogin;
using MinhasReceitasApp.Application.UseCases.User.Profile;
using MinhasReceitasApp.Application.UseCases.User.Register;
using MinhasReceitasApp.Application.UseCases.User.Update;

namespace MinhasReceitasApp.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddUseCase(services);
        AddAutoMapper(services);
        AddPasswordEncripter(services, configuration);
    }

    private static void AddUseCase(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
    }

    private static void AddAutoMapper(IServiceCollection services)
    {

        services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }).CreateMapper());
    }
    private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
    {
        var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        services.AddScoped(option => new PasswordEncripter(additionalKey!));
    }
}
