using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace MinhasReceitasApp.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MinhasReceitasAppException minhasReceitasAppException)
            HandleProjectException(minhasReceitasAppException, context);
        else
        {
            ThrowUnknowException(context);
        }
    }

    private static void HandleProjectException(MinhasReceitasAppException minhasReceitasAppException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)minhasReceitasAppException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(minhasReceitasAppException.GetErrorMessages()));
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson("Erro desconhecido."));
    }
}
