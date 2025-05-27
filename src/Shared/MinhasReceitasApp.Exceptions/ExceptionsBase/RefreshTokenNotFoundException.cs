namespace MinhasReceitasApp.Exceptions.ExceptionsBase;

public class RefreshTokenNotFoundException : MinhasReceitasAppException
{
    public RefreshTokenNotFoundException() : base("Refresh token not found.")
    { }
}
