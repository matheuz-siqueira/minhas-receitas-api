using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhasReceitasApp.Application.Services.AutoMapper;
using MinhasReceitasApp.Application.Services.Cryptography;
using MinhasReceitasApp.Application.UseCases.User.Register;

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
    }

    private static void AddAutoMapper(IServiceCollection services)
    {

        services.AddScoped(option =>  new AutoMapper.MapperConfiguration(options => 
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
