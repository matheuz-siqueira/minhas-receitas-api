using System.Net;

namespace MinhasReceitasApp.Exceptions.ExceptionsBase;

public abstract class MinhasReceitasAppException : SystemException
{
    public MinhasReceitasAppException(string message) : base(message) { }

    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();

}
