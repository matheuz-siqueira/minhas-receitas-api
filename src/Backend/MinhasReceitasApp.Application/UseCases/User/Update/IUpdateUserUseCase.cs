using MinhasReceitasApp.Communication.Requests;

namespace MinhasReceitasApp.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}
