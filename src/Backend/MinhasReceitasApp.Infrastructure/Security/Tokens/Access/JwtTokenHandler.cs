using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MinhasReceitasApp.Infrastructure.Security.Tokens.Access;

public abstract class JwtTokenHandler
{
    protected static SymmetricSecurityKey SecurityKey(string signingKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signingKey);
        return new SymmetricSecurityKey(bytes);
    }
}
