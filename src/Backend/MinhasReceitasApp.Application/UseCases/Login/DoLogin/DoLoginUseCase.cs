using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Repositories.User;
using MinhasReceitasApp.Domain.Security.Cryptography;
using MinhasReceitasApp.Domain.Security.Tokens;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace MinhasReceitasApp.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    public DoLoginUseCase(
        IUserReadOnlyRepository repository,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessTokenGenerator;
    }
    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var user = await _repository.GetByEmail(request.Email);

        if (user is null || _passwordEncripter.IsValid(request.Password, user.Password).IsFalse())
            throw new InvalidLoginException();

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokenJson
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
            }
        };


    }
}
