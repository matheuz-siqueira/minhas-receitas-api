using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Mvc;
using MinhasReceitasApp.Application.UseCases.Token.RefreshToken;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.API.Controllers;

public class TokenController : MInhasReceitasAppBaseController
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokenJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] IUserRefreshTokenUseCase useCase,
        [FromBody] RequestNewTokenJson request)
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }

}
