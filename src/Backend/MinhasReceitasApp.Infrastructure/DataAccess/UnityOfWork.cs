using MinhasReceitasApp.Domain.Repositories;

namespace MinhasReceitasApp.Infrastructure.DataAccess;

public class UnityOfWork : IUnityOfWork
{
    private readonly MinhasReceitasAppDbContext _dbContext; 
    public UnityOfWork(MinhasReceitasAppDbContext dbContext) => _dbContext = dbContext; 

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
