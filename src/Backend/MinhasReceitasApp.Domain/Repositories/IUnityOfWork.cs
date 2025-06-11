namespace MinhasReceitasApp.Domain.Repositories;

public interface IUnityOfWork
{
    public Task Commit();     
}
