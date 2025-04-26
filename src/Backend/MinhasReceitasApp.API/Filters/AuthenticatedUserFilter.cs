using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Repositories.User;
using MinhasReceitasApp.Domain.Security.Tokens;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace MinhasReceitasApp.API.Filters;

public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator;
    private readonly IUserReadOnlyRepository _userRepository;

    public AuthenticatedUserFilter(
        IAccessTokenValidator accessTokenValidator,
        IUserReadOnlyRepository userRepository)
    {
        _accessTokenValidator = accessTokenValidator; 
        _userRepository = userRepository; 
    }
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokneOnRequest(context); 
            var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token); 
            var exist = await _userRepository.ExistActiveUserWithIdentifier(userIdentifier); 
            if(exist.IsFalse())
            {
                throw new MinhasReceitasAppException("user does not have permission to access this resource.");
            }
        }
        catch(SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("Token is expired.")
            {
                TokenIsExpiret = true,
            });
        }
        catch(MinhasReceitasAppException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("user does not have permission to access this resource."));
        }
    }

    private static string TokneOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if(string.IsNullOrWhiteSpace(authentication))
            throw new MinhasReceitasAppException("Request don't have token."); 
        
        return authentication["Bearer ".Length..].Trim();  
    }   
}
