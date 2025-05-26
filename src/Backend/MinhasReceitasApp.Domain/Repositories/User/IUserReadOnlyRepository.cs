namespace MinhasReceitasApp.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistActiveWithEmail(string email);
    public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
    public Task<Entities.User?> GetByEmail(string email);
}
