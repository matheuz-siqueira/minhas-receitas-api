using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.Application.UseCases.Recipe.GetById;

public interface IGetRecipeByIdUseCase
{
    public Task<ResponseRecipeJson> Execute(long recipeId);
}
