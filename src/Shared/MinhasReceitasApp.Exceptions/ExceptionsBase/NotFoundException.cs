namespace MinhasReceitasApp.Exceptions.ExceptionsBase;

public class NotFoundException : MinhasReceitasAppException
{
    public NotFoundException(string message) : base(message)
    { }
}
