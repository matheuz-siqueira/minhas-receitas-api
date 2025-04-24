using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MinhasReceitasApp.Application.UseCases.Login.DoLogin;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace UseCases.Test.DoLogin;

public class DoLoginUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();
        var usecase = CreateUseCase(user);

        var result = await usecase.Execute(new RequestLoginJson
        {
            Email = user.Email, 
            Password = password
        });

        result.Should().NotBeNull(); 
        result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
    }

    [Fact]
    public async Task Error_Invalid_User()
    {
        var request = RequestLoginJsonBuilder.Build(); 

        var usecase = CreateUseCase(); 

        Func<Task> act = async () => { await usecase.Execute(request); }; 

        await act.Should().ThrowAsync<InvalidLoginException>()
            .Where(e => e.Message.Equals("Email and/or password invalid."));
    }

    private static DoLoginUseCase CreateUseCase(MinhasReceitasApp.Domain.Entities.User? user = null)
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder(); 

        if(user is not null)
            userReadOnlyRepositoryBuilder.GetByEmailAndPassword(user); 

        return new DoLoginUseCase(userReadOnlyRepositoryBuilder.Build(), passwordEncripter);
    }
}
