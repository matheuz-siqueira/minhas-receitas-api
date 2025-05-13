using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MinhasReceitasApp.Application.UseCases.Recipe.Register;
using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace UseCases.Test.Recipe.Register;

public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestRecipeJsonBuilder.Build();
        var useCase = CreateUseCase(user);
        var result = await useCase.Execute(request);
        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
                e.ErrorMessages.Contains("title cannot be empty."));
    }

    private static RegisterRecipeUseCase CreateUseCase(MinhasReceitasApp.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Build();
        var unityOfWork = UnityOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = RecipeWriteOnlyRepositoryBuilder.Build();

        return new RegisterRecipeUseCase(repository, loggedUser, unityOfWork, mapper);

    }
}
