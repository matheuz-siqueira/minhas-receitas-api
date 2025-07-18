using MinhasReceitasApp.Domain.Security.Tokens;

namespace MinhasReceitasApp.API.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor; 
    }
    public string Value()
    {
        var authentication = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authentication["Bearer ".Length..].Trim();
    }
}
