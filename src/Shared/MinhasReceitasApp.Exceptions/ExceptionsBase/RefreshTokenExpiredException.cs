namespace MinhasReceitasApp.Exceptions.ExceptionsBase;

public class RefreshTokenExpiredException : MinhasReceitasAppException
{
    public RefreshTokenExpiredException() : base("Refresh token is expired.")
    { }
}
