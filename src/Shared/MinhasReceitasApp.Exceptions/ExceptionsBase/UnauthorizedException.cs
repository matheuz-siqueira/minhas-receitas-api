using System.Net;

namespace MinhasReceitasApp.Exceptions.ExceptionsBase;

public class UnauthorizedException : MinhasReceitasAppException
{
    public UnauthorizedException(string message) : base(message)
    { }
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
