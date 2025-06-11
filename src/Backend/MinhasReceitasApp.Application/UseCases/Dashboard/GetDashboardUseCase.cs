
using AutoMapper;
using MinhasReceitasApp.Application.Extensions;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Domain.Services.Storage;

namespace MinhasReceitasApp.Application.UseCases.Dashboard;

public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IBlobStorageService _blobStorageService;
    public GetDashboardUseCase(
        IRecipeReadOnlyRepository repository,
        IMapper mapper,
        ILoggedUser loggedUser,
        IBlobStorageService blobStorageService)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
        _blobStorageService = blobStorageService;
    }
    public async Task<ResponseRecipesJson> Execute()
    {
        var loggedUser = await _loggedUser.User();

        var recipes = await _repository.GetForDashboard(loggedUser);

        return new ResponseRecipesJson
        {
            Recipes = await recipes.MapToShortRecipeJson(loggedUser, _blobStorageService, _mapper)
        };
    }
}
