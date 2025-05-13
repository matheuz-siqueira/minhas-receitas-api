using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.Application.UseCases.Recipe.Register;

public interface IRegisterRecipeUseCase
{
    public Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request);
}
