using AutoMapper;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Services.Storage;

namespace MinhasReceitasApp.Application.Extensions;

public static class RecipeListExtension
{
    public static async Task<IList<ResponseShortRecipeJson>> MapToShortRecipeJson(
        this IList<Recipe> recipes,
        User user,
        IBlobStorageService blobStorageService,
        IMapper mapper)
    {
        var result = recipes.Select(async recipe =>
        {
            var response = mapper.Map<ResponseShortRecipeJson>(recipe);
            if (recipe.ImageIdentifier.NotEmpty())
            {
                response.ImageUrl = await blobStorageService.GetFileUrl(user, recipe.ImageIdentifier);
            }

            return response;
        });

        return await Task.WhenAll(result);
    }
}
