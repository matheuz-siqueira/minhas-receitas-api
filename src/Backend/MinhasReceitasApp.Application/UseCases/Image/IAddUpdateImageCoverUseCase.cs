using Microsoft.AspNetCore.Http;

namespace MinhasReceitasApp.Application.UseCases.Image;

public interface IAddUpdateImageCoverUseCase
{
    Task Execute(long recipeId, IFormFile file);
}
