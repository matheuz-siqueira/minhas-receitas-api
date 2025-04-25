namespace MinhasReceitasApp.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    public string Generate(Guid userIdentifier); 
}
