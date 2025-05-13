using Microsoft.AspNetCore.Mvc;
using MinhasReceitasApp.API.Filters;

namespace MinhasReceitasApp.API.Attributes;

public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {

    }
}
