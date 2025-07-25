using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MinhasReceitasApp.Application.UseCases.Recipe.Update;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace UseCases.Test.Recipe.Update;

public class UpdateRecipeUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var request = RequestRecipeJsonBuilder.Build();
        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => await useCase.Execute(recipe.Id, request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Recipe_NotFound()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestRecipeJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(recipeId: 1000, request);

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Equals("Recipe not found."));
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => await useCase.Execute(recipe.Id, request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                e.GetErrorMessages().Contains("title cannot be empty."));
    }

    private static UpdateRecipeUseCase CreateUseCase(
        MinhasReceitasApp.Domain.Entities.User user,
        MinhasReceitasApp.Domain.Entities.Recipe? recipe = null)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var unityOfWork = UnityOfWorkBuilder.Build();
        var repository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build();

        return new UpdateRecipeUseCase(loggedUser, mapper, unityOfWork, repository);

    }
}
