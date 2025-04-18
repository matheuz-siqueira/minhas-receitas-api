namespace MinhasReceitasApp.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistActiveWithEmail(string email); 
}
