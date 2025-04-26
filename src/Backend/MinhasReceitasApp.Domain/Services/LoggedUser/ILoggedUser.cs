using MinhasReceitasApp.Domain.Entities;

namespace MinhasReceitasApp.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User();
}
