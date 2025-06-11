using System.Net;

namespace MinhasReceitasApp.Exceptions.ExceptionsBase;

public class InvalidLoginException : MinhasReceitasAppException
{
    public InvalidLoginException() : base("Email and/or password invalid.")
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
