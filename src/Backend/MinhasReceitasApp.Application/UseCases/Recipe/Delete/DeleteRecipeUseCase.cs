
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Domain.Services.Storage;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace MinhasReceitasApp.Application.UseCases.Recipe.Delete;

public class DeleteRecipeUseCase : IDeleteRecipeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repositoryRead;
    private readonly IRecipeWriteOnlyRepository _repositoryWrite;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IBlobStorageService _blobStorageService;
    public DeleteRecipeUseCase(
        ILoggedUser loggedUser,
        IRecipeReadOnlyRepository repositoryRead,
        IRecipeWriteOnlyRepository repositoryWrite,
        IUnityOfWork unityOfWork,
        IBlobStorageService blobStorageService)
    {
        _loggedUser = loggedUser;
        _repositoryRead = repositoryRead;
        _repositoryWrite = repositoryWrite;
        _unityOfWork = unityOfWork;
        _blobStorageService = blobStorageService;
    }
    public async Task Execute(long recipeId)
    {
        var loggedUser = await _loggedUser.User();

        var recipe = await _repositoryRead.GetById(loggedUser, recipeId);
        if (recipe is null)
            throw new NotFoundException("Recipe not found.");

        if (recipe.ImageIdentifier.NotEmpty())
            await _blobStorageService.Delete(loggedUser, recipe.ImageIdentifier);

        await _repositoryWrite.Delete(recipeId);
        await _unityOfWork.Commit();
    }
}
