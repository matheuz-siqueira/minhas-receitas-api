namespace MinhasReceitasApp.Exceptions.ExceptionsBase;

public class InvalidLoginException : MinhasReceitasAppException
{
    public InvalidLoginException() : base("Email and/or password invalid")
    {
    }
}
