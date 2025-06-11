
using Microsoft.AspNetCore.Http;
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Domain.Services.Storage;
using MinhasReceitasApp.Exceptions.ExceptionsBase;
using MinhasReceitasApp.Application.Extensions;
using MinhasReceitasApp.Domain.Extensions;

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
        (var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();
        if (isValidImage.IsFalse())
        {
            throw new ErrorOnValidationException(["only png, jpg and jpeg images are accepted."]);
        }

        if (string.IsNullOrEmpty(recipe.ImageIdentifier))
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}";
            _repository.Update(recipe);
            await _unityOfWork.Commit();
        }
        await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier);

    }


}
