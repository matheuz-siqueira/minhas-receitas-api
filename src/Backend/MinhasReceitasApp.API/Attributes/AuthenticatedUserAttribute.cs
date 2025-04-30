using Microsoft.AspNetCore.Mvc;
using MinhasReceitasApp.API.Filters;

namespace MinhasReceitasApp.API.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {

    }
}
