using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    public Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request);
}
