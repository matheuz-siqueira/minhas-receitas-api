using Microsoft.AspNetCore.Mvc;
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
}
