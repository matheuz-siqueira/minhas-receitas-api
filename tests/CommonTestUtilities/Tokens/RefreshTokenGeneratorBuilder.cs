using MinhasReceitasApp.Domain.Security.Tokens;
using MinhasReceitasApp.Infrastructure.Security.Tokens.Refresh;

namespace CommonTestUtilities.Tokens;

public class RefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build() => new RefreshTokenGenerator();
}
