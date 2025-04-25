using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinhasReceitasApp.Infrastructure.DataAccess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private MinhasReceitasApp.Domain.Entities.User _user = default!;  
    private string _password = string.Empty; 
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services => 
            {
                var descriptor = services
                    .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MinhasReceitasAppDbContext>));
                if(descriptor is not null)
                    services.Remove(descriptor); 

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<MinhasReceitasAppDbContext>(options => 
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider); 
                }) ;

                using var scope = services.BuildServiceProvider().CreateScope(); 
                var dbContext = scope.ServiceProvider.GetRequiredService<MinhasReceitasAppDbContext>();
                
                dbContext.Database.EnsureDeleted();

                StartDatabase(dbContext); 

            });
    }

    public string GetEmail() => _user.Email; 
    public string GetPassword() => _password; 
    public string GetName() => _user.Name; 

    private void StartDatabase(MinhasReceitasAppDbContext dbContext)
    {
        (_user,  _password) = UserBuilder.Build();

        dbContext.Users.Add(_user); 
        dbContext.SaveChanges(); 
    }
}
