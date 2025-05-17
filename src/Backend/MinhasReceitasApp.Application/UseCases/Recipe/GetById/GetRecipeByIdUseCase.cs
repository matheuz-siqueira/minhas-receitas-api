using AutoMapper;
using FluentValidation.Validators;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace MinhasReceitasApp.Application.UseCases.Recipe.GetById;

public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;
    public GetRecipeByIdUseCase(
        IMapper mapper,
        ILoggedUser loggedUser,
        IRecipeReadOnlyRepository repository)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _repository = repository;
    }
    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        var loggedUser = await _loggedUser.User();
        var recipe = await _repository.GetById(loggedUser, recipeId);

        if (recipe is null)
            throw new NotFoundException("Recipe not found.");

        return _mapper.Map<ResponseRecipeJson>(recipe);
    }
}
