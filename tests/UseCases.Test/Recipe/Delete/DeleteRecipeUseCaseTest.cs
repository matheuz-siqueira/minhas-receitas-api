using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using MinhasReceitasApp.Application.UseCases.Recipe.Delete;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace UseCases.Test.Recipe.Delete;

public class DeleteRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => { await useCase.Execute(recipe.Id); };

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Recipe_NotFound()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(1); };

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Equals("Recipe not found."));
    }

    
    private static DeleteRecipeUseCase CreateUseCase(
        MinhasReceitasApp.Domain.Entities.User user,
        MinhasReceitasApp.Domain.Entities.Recipe? recipe = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repositoryRead = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();
        var repositoryWrite = RecipeWriteOnlyRepositoryBuilder.Build();
        var unityOfWork = UnityOfWorkBuilder.Build();

        return new DeleteRecipeUseCase(loggedUser, repositoryRead, repositoryWrite, unityOfWork);
    }
}
