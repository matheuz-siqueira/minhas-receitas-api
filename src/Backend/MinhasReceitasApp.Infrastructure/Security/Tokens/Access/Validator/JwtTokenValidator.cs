using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MinhasReceitasApp.Domain.Security.Tokens;

namespace MinhasReceitasApp.Infrastructure.Security.Tokens.Access.Validator;

public class JwtTokenValidator : JwtTokenHandler, IAccessTokenValidator
{
    private readonly string _signingKey; 
    public JwtTokenValidator(string signingKey) => _signingKey = signingKey; 
    public Guid ValidateAndGetUserIdentifier(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, 
            ValidateIssuer = false, 
            IssuerSigningKey = SecurityKey(_signingKey),
            ClockSkew = new TimeSpan(0), 

        };

        var tokenHandler = new JwtSecurityTokenHandler(); 
        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
        var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value; 
        
        return Guid.Parse(userIdentifier); 
    }
}
