using System.Net;

namespace MinhasReceitasApp.Exceptions.ExceptionsBase;

public class NotFoundException : MinhasReceitasAppException
{
    public NotFoundException(string message) : base(message)
    { }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
