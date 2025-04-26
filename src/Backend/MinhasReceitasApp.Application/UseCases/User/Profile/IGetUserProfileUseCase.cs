using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUseProfileJson> Execute();
}
