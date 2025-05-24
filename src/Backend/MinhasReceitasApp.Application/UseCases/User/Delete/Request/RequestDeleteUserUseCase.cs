
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.User;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Domain.Services.ServiceBus;

namespace MinhasReceitasApp.Application.UseCases.User.Delete.Request;

public class RequestDeleteUserUseCase : IRequestDeleteUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IDeleteUserQueue _queue;


    public RequestDeleteUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IUnityOfWork unityOfWork,
        IDeleteUserQueue queue)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unityOfWork = unityOfWork;
        _queue = queue;
    }
    public async Task Execute()
    {
        var loggedUser = await _loggedUser.User();

        var user = await _repository.GetById(loggedUser.Id);

        user.Active = false;
        _repository.Update(user);

        await _unityOfWork.Commit();

        await _queue.SendMessage(loggedUser);
    }
}
