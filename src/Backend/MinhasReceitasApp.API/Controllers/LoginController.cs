using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using MinhasReceitasApp.Application.UseCases.Login.DoLogin;
using MinhasReceitasApp.Application.UseCases.Login.External;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.API.Controllers;

public class LoginController : MInhasReceitasAppBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase, [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }

    [HttpGet]
    [Route("google")]
    public async Task<IActionResult> LoginGoogle(
        string returnUrl,
        [FromServices] IExternalLoginUseCase useCase)
    {
        var authenticate = await Request.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (IsNotAuthenticated(authenticate))
        {
            return Challenge(GoogleDefaults.AuthenticationScheme);
        }
        else
        {
            var claims = authenticate.Principal!.Identities.First().Claims;
            var name = claims.First(c => c.Type == ClaimTypes.Name).Value;
            var email = claims.First(c => c.Type == ClaimTypes.Email).Value;

            var token = await useCase.Execute(name, email);

            return Redirect($"{returnUrl}/{token}");
        }
    }
}
