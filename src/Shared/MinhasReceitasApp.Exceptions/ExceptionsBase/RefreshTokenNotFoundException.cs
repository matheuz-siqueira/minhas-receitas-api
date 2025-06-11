using System.Net;

namespace MinhasReceitasApp.Exceptions.ExceptionsBase;

public class RefreshTokenNotFoundException : MinhasReceitasAppException
{
    public RefreshTokenNotFoundException() : base("Refresh token not found.")
    { }
    public override IList<string> GetErrorMessages() => [Message];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
