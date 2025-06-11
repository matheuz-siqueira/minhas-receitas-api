using System.Threading.Tasks;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using FluentAssertions;
using MinhasReceitasApp.Application.UseCases.User.Profile;
using MinhasReceitasApp.Domain.Entities;

namespace UseCases.Test.User.Profile;

public class GetUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var _) = UserBuilder.Build(); 
        var useCase = CreateUseCase(user); 

        var result = await useCase.Execute(); 

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
    }
    private static GetUserProfileUseCase CreateUseCase(MinhasReceitasApp.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Build(); 
        var loggedUser = LoggedUserBuilder.Build(user); 

        return new GetUserProfileUseCase(loggedUser, mapper); 
    }
}
