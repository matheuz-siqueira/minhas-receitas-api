namespace MinhasReceitasApp.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : MinhasReceitasAppException
{
    public IList<string> ErrorMessages { get; set; }
    public ErrorOnValidationException(IList<string> errorMessages)
    {
        ErrorMessages = errorMessages; 
    }
}
