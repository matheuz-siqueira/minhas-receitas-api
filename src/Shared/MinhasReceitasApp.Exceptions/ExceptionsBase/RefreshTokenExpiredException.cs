using System.Net;

namespace MinhasReceitasApp.Exceptions.ExceptionsBase;

public class RefreshTokenExpiredException : MinhasReceitasAppException
{
    public RefreshTokenExpiredException() : base("Refresh token is expired.")
    { }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
