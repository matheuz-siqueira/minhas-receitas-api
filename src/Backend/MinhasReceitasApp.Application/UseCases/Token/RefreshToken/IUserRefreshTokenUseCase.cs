using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;

namespace MinhasReceitasApp.Application.UseCases.Token.RefreshToken;

public interface IUserRefreshTokenUseCase
{
    public Task<ResponseTokenJson> Execute(RequestNewTokenJson request);
}
