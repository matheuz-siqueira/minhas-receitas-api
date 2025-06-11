using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.Application.UseCases.Dashboard;

public interface IGetDashboardUseCase
{
    Task<ResponseRecipesJson> Execute();
}
