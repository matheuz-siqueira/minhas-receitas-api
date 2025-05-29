using System.Threading.Tasks;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MinhasReceitasApp.Application.UseCases.User.Update;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);

    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);
        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                e.GetErrorMessages().Contains("O nome não pode ser vazio."));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);

    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUseCase(user);
        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                e.GetErrorMessages().Contains("O Email não pode ser vazio."));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);
        Func<Task> act = async () => { await useCase.Execute(request); };

        await act.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.GetErrorMessages().Count == 1 &&
                e.GetErrorMessages().Contains("Esse email já está cadastrado."));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }

    private static UpdateUserUseCase CreateUseCase(MinhasReceitasApp.Domain.Entities.User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var unityOfWork = UnityOfWorkBuilder.Build();
        var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();

        if (string.IsNullOrEmpty(email).IsFalse())
            userReadOnlyRepository.ExistActiveUserWithEmail(email!);

        return new UpdateUserUseCase(loggedUser, userUpdateOnlyRepository, userReadOnlyRepository.Build(), unityOfWork);
    }
}
