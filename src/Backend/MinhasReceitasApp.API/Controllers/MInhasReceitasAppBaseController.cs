using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MinhasReceitasApp.Domain.Extensions;

namespace MinhasReceitasApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MInhasReceitasAppBaseController : ControllerBase
{
    protected static bool IsNotAuthenticated(AuthenticateResult authenticate)
    {
        return authenticate.Succeeded.IsFalse()
            || authenticate.Principal is null
            || authenticate.Principal.Identities.Any(id => id.IsAuthenticated).IsFalse();
    }
}
