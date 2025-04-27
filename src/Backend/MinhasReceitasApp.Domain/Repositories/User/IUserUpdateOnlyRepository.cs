using MinhasReceitasApp.Domain.Entities;

namespace MinhasReceitasApp.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    public void Update(Entities.User user);
    public Task<Entities.User> GetById(long id);
}
