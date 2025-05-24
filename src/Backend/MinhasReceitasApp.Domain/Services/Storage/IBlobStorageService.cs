using MinhasReceitasApp.Domain.Entities;

namespace MinhasReceitasApp.Domain.Services.Storage;

public interface IBlobStorageService
{
    public Task Upload(User user, Stream file, string fileName);
    public Task<string> GetFileUrl(User user, string fileName);
    public Task Delete(User user, string fileName);
}
