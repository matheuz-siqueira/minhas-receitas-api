using Microsoft.EntityFrameworkCore;
using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Domain.Repositories.User;

namespace MinhasReceitasApp.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly MinhasReceitasAppDbContext _dbContext;

    public UserRepository(MinhasReceitasAppDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) => await _dbContext.Users.AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);

    public async Task<bool> ExistActiveWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Password.Equals(password));
    }

    public async Task<User> GetById(long id)
    {
        return await _dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public void Update(User user) => _dbContext.Users.Update(user);
}
