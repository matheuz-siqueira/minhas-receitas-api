using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Domain.Repositories.Recipe;

namespace MinhasReceitasApp.Infrastructure.DataAccess.Repositories;

public class RecipeRepository : IRecipeWriteOnlyRepository
{
    private readonly MinhasReceitasAppDbContext _dbContext;
    public RecipeRepository(MinhasReceitasAppDbContext dbContext) => _dbContext = dbContext;
    public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);
}
