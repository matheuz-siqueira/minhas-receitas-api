using MinhasReceitasApp.Domain.Security.Tokens;
using MinhasReceitasApp.Infrastructure.Security.Tokens.Access.Generator;

namespace CommonTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "cl94bWhKwqMrSy12WFNUOz0xIXksV3kt");
}
