using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using MinhasReceitasApp.Application.UseCases.Recipe.GetById;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace UseCases.Test.Recipe.GetById;

public class GetByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);
        var result = await useCase.Execute(recipe.Id);

        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().Be(recipe.Title);
        result.ImageUrl.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Recipe_NotFound()
    {
        (var user, _) = UserBuilder.Build();


        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(recipeId: 1000); };
        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Equals("Recipe not found."));
    }

    private static GetRecipeByIdUseCase CreateUseCase(
        MinhasReceitasApp.Domain.Entities.User user,
        MinhasReceitasApp.Domain.Entities.Recipe? recipe = null)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeReadOnlyRepositoryBuilder().GetById(user, recipe).Build();
        var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipe?.ImageIdentifier).Build();

        return new GetRecipeByIdUseCase(mapper, loggedUser, repository, blobStorage);
    }
}
