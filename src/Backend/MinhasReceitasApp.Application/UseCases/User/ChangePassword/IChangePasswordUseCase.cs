using MinhasReceitasApp.Communication.Requests;

namespace MinhasReceitasApp.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    public Task Execute(RequestChangePasswordJson request);
}
