using MinhasReceitasApp.Domain.Security.Tokens;

namespace MinhasReceitasApp.Infrastructure.Security.Tokens.Refresh;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
}
