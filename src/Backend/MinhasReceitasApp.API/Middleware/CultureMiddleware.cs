using System.Globalization;
using MinhasReceitasApp.Domain.Extensions;

namespace MinhasReceitasApp.API.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    public CultureMiddleware(RequestDelegate next)
    {
        _next = next; 
    }
    public async Task Invoke(HttpContext context)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList(); 

        var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        var cultureInfo = new CultureInfo("en");
        
        if(requestedCulture.NotEmpty() && 
            supportedLanguages.Exists(c => c.Name == requestedCulture))
        {
            cultureInfo = new CultureInfo(requestedCulture);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo; 
    
        await _next(context);
    }
}
