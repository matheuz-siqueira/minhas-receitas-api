using Microsoft.AspNetCore.Mvc;
using MinhasReceitasApp.API.Attributes;
using MinhasReceitasApp.Application.UseCases.User.Profile;
using MinhasReceitasApp.Application.UseCases.User.Register;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.API.Controllers;

public class UserController : MInhasReceitasAppBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var result = await useCase.Execute(request); 

        return Created(string.Empty, result);  
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseUseProfileJson), StatusCodes.Status200OK)]
    [AuthenticatedUser]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var result = await useCase.Execute(); 
        return Ok(result); 
    }
}
