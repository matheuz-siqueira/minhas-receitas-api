using MinhasReceitasApp.Domain.Dtos;
using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using Moq;

namespace CommonTestUtilities.Repositories;

public class RecipeReadOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeReadOnlyRepository> _repository;

    public RecipeReadOnlyRepositoryBuilder() => _repository = new Mock<IRecipeReadOnlyRepository>();
    public RecipeReadOnlyRepositoryBuilder Filter(User user, IList<Recipe> recipes)
    {
        _repository.Setup(repository => repository.Filter(user, It.IsAny<FilterRecipesDto>())).ReturnsAsync(recipes);
        return this;
    }

    public IRecipeReadOnlyRepository Build() => _repository.Object;
}
