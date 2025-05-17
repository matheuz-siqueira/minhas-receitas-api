using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.Application.UseCases.Recipe.Update;

public interface IUpdateRecipeUseCase
{
    Task Execute(long recipeId, RequestRecipeJson request);
}
