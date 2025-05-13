
using Microsoft.AspNetCore.Mvc;
using MinhasReceitasApp.API.Attributes;
using MinhasReceitasApp.Application.UseCases.Recipe.Register;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.API.Controllers;

[AuthenticatedUser]
public class RecipeController : MInhasReceitasAppBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredRecipeJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterRecipeUseCase useCase,
        [FromBody] RequestRecipeJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}
