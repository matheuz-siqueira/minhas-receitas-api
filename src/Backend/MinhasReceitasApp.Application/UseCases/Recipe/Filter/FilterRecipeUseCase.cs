using AutoMapper;
using MinhasReceitasApp.Application.Extensions;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Domain.Services.Storage;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace MinhasReceitasApp.Application.UseCases.Recipe.Filter;

public class FilterRecipeUseCase : IFilterRecipeUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IBlobStorageService _blobStorageService;
    public FilterRecipeUseCase(
        IMapper mapper,
        ILoggedUser loggedUser,
        IRecipeReadOnlyRepository repository,
        IBlobStorageService blobStorageService)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _repository = repository;
        _blobStorageService = blobStorageService;
    }
    public async Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request)
    {
        Validate(request);
        var loggedUser = await _loggedUser.User();

        var filters = new Domain.Dtos.FilterRecipesDto
        {
            RecipeTitle_Ingredients = request.RecipeTitle_Ingredient,
            CookingTimes = request.CookingTimes.Distinct().Select(c => (Domain.Enums.CookingTime)c).ToList(),
            DishTypes = request.DishTypes.Distinct().Select(c => (Domain.Enums.DishType)c).ToList()
        };

        var recipes = await _repository.Filter(loggedUser, filters);

        return new ResponseRecipesJson
        {
            Recipes = await recipes.MapToShortRecipeJson(loggedUser, _blobStorageService, _mapper)
        };
    }

    private static void Validate(RequestFilterRecipeJson request)
    {
        var validator = new FilterRecipeValidator();
        var result = validator.Validate(request);
        if (result.IsValid.IsFalse())
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).Distinct().ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
