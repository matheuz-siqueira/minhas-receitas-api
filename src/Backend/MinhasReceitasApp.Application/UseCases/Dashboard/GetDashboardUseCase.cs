
using AutoMapper;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using MinhasReceitasApp.Domain.Services.LoggedUser;

namespace MinhasReceitasApp.Application.UseCases.Dashboard;

public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    public GetDashboardUseCase(
        IRecipeReadOnlyRepository repository,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseRecipesJson> Execute()
    {
        var loggedUser = await _loggedUser.User();

        var recipes = await _repository.GetForDashboard(loggedUser);

        return new ResponseRecipesJson
        {
            Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes)
        };
    }
}
