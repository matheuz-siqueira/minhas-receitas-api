using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
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
        result.Tokens.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
        result.Tokens.AccessToken.Should().NotBeNullOrWhiteSpace();
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
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var unityOfWork = UnityOfWorkBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        if (user is not null)
            userReadOnlyRepositoryBuilder.GetByEmail(user);

        return new DoLoginUseCase(
            userReadOnlyRepositoryBuilder.Build(), passwordEncripter,
            accessTokenGenerator, refreshTokenGenerator, tokenRepository, unityOfWork
        );
    }
}
