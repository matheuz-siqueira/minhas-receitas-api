using Microsoft.AspNetCore.Mvc;
using MinhasReceitasApp.API.Attributes;
using MinhasReceitasApp.Application.UseCases.Dashboard;
using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.API.Controllers;

[AuthenticatedUser]
public class DashboardController : MInhasReceitasAppBaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get([FromServices] IGetDashboardUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Recipes.Any())
            return Ok(response);
        return NoContent();

    }
}
