
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.User;
using MinhasReceitasApp.Domain.Services.Storage;

namespace MinhasReceitasApp.Application.UseCases.User.Delete.Delete;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly IUserDeleteOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public DeleteUserAccountUseCase(IUnityOfWork unityOfWork,
        IBlobStorageService blobStorageService,
        IUserDeleteOnlyRepository repository)
    {
        _unityOfWork = unityOfWork;
        _blobStorageService = blobStorageService;
        _repository = repository;
    }

    public async Task Execute(Guid userIdentifier)
    {
        await _blobStorageService.DeleteContainer(userIdentifier);

        await _repository.DeleteAccount(userIdentifier);

        await _unityOfWork.Commit();
    }
}
