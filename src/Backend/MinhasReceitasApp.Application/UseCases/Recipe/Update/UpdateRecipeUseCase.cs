using System.Linq.Expressions;
using AutoMapper;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace MinhasReceitasApp.Application.UseCases.Recipe.Update;

public class UpdateRecipeUseCase : IUpdateRecipeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IRecipeUpdateOnlyRepository _repository;
    public UpdateRecipeUseCase(
        ILoggedUser loggedUser,
        IMapper mapper,
        IUnityOfWork unityOfWork,
        IRecipeUpdateOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
        _unityOfWork = unityOfWork;
        _repository = repository;
    }
    public async Task Execute(long recipeId, RequestRecipeJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.User();
        var recipe = await _repository.GetById(loggedUser, recipeId);

        if (recipe is null)
            throw new NotFoundException("Recipe not found.");

        recipe.Ingredients.Clear();
        recipe.Instructions.Clear();
        recipe.DishTypes.Clear();

        _mapper.Map(request, recipe);

        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();

        for (var index = 0; index < instructions.Count; index++)
            instructions[index].Step = index + 1;

        recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

        _repository.Update(recipe);

        await _unityOfWork.Commit();
    }

    private static void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);
        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}
