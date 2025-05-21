using MinhasReceitasApp.Domain.Entities;

namespace MinhasReceitasApp.Domain.Services.Storage;

public interface IBlobStorageService
{
    Task Upload(User user, Stream file, string fileName);

    Task<string> GetImageUrl(User user, string fileName);
}
