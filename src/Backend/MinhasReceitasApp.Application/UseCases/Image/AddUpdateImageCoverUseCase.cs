
using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using Microsoft.AspNetCore.Http;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Domain.Services.Storage;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace MinhasReceitasApp.Application.UseCases.Image;

public class AddUpdateImageCoverUseCase : IAddUpdateImageCoverUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeUpdateOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public AddUpdateImageCoverUseCase(
        ILoggedUser loggedUser,
        IRecipeUpdateOnlyRepository repository,
        IUnityOfWork unityOfWork,
        IBlobStorageService blobStorageService)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unityOfWork = unityOfWork;
        _blobStorageService = blobStorageService;
    }
    public async Task Execute(long recipeId, IFormFile file)
    {
        var loggedUser = await _loggedUser.User();

        var recipe = await _repository.GetById(loggedUser, recipeId);

        if (recipe is null)
            throw new NotFoundException("Recipe not found.");

        var fileStream = file.OpenReadStream();
        if (fileStream.Is<PortableNetworkGraphic>().IsFalse() &&
            fileStream.Is<JointPhotographicExpertsGroup>().IsFalse())
        {
            throw new ErrorOnValidationException(["only png, jpg and jpeg images are accepted."]);
        }

        if (string.IsNullOrEmpty(recipe.ImageIdentifier))
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            _repository.Update(recipe);
            await _unityOfWork.Commit();
        }

        fileStream.Position = 0;

        await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier);


    }


}
