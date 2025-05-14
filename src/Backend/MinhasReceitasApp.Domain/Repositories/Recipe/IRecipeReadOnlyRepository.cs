using MinhasReceitasApp.Domain.Dtos;

namespace MinhasReceitasApp.Domain.Repositories.Recipe;

public interface IRecipeReadOnlyRepository
{
    Task<IList<Entities.Recipe>> Filter(Entities.User user, FilterRecipesDto filters);
}
