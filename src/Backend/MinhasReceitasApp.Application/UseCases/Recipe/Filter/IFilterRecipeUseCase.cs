using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.Application.UseCases.Recipe.Filter;

public interface IFilterRecipeUseCase
{
    public Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request);
}
