namespace MinhasReceitasApp.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistActiveWithEmail(string email);
    public Task<Entities.User?> GetByEmailAndPassword(string email, string password);
    public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
}
