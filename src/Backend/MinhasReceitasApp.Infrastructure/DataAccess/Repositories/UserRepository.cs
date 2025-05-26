using Microsoft.EntityFrameworkCore;
using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Domain.Repositories.User;

namespace MinhasReceitasApp.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository, IUserDeleteOnlyRepository
{
    private readonly MinhasReceitasAppDbContext _dbContext;

    public UserRepository(MinhasReceitasAppDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);



    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) => await _dbContext.Users.AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);

    public async Task<bool> ExistActiveWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);

    public async Task<User> GetById(long id)
    {
        return await _dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public void Update(User user) => _dbContext.Users.Update(user);
    public async Task DeleteAccount(Guid userIdentifier)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserIdentifier == userIdentifier);

        if (user is null)
            return;

        var recipes = _dbContext.Recipes.Where(recipes => recipes.UserId == user.Id);

        _dbContext.Recipes.RemoveRange(recipes);

        _dbContext.Users.Remove(user);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext
           .Users
           .AsNoTracking()
           .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email));
    }
}
