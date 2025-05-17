using Microsoft.EntityFrameworkCore;
using MinhasReceitasApp.Domain.Dtos;
using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Repositories.Recipe;

namespace MinhasReceitasApp.Infrastructure.DataAccess.Repositories;

public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository
{
    private readonly MinhasReceitasAppDbContext _dbContext;
    public RecipeRepository(MinhasReceitasAppDbContext dbContext) => _dbContext = dbContext;
    public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);

    public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
    {
        var query = _dbContext.Recipes
            .AsNoTracking()
            .Include(recipe => recipe.Ingredients)
            .Where(recipe => recipe.Active && recipe.UserId == user.Id);


        if (filters.Difficulties.Any())
        {
            query = query.Where(
                recipe => recipe.Difficulty.HasValue && filters.Difficulties.Contains(recipe.Difficulty.Value));
        }

        if (filters.CookingTimes.Any())
        {
            query = query.Where(
                recipe => recipe.CookingTime.HasValue && filters.CookingTimes.Contains(recipe.CookingTime.Value));
        }

        if (filters.DishTypes.Any())
        {
            query = query.Where(
                recipe => recipe.DishTypes.Any(dishType => filters.DishTypes.Contains(dishType.Type)));
        }

        if (filters.RecipeTitle_Ingredients.NotEmpty())
        {
            query = query.Where(recipe => recipe.Title.Contains(filters.RecipeTitle_Ingredients) ||
                recipe.Ingredients
                    .Any(ingredient => ingredient.Item.Contains(filters.RecipeTitle_Ingredients)));
        }




        return await query.ToListAsync();

    }

    public async Task<Recipe?> GetById(User user, long recipeId)
    {
        return await _dbContext.Recipes
            .AsNoTracking()
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.DishTypes)
            .FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == user.Id);
    }
}
