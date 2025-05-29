using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MinhasReceitasApp.Application.UseCases.User.ChangePassword;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = password;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_New_Password_Empty()
    {
        (var user, var password) = UserBuilder.Build();

        var request = new RequestChangePasswordJson
        {
            Password = password,
            NewPassword = string.Empty
        };

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                e.GetErrorMessages().Contains("A senha não pode ser vazia."));
    }

    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        (var user, var password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => { await useCase.Execute(request); };

        await act.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.GetErrorMessages().Count == 1 &&
                e.GetErrorMessages().Contains("A senha atual está incorreta."));
    }
    private static ChangePasswordUseCase CreateUseCase(MinhasReceitasApp.Domain.Entities.User user)
    {
        var unityOfWork = UnityOfWorkBuilder.Build();
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncripter = PasswordEncripterBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, userUpdateRepository, passwordEncripter, unityOfWork);
    }
}
