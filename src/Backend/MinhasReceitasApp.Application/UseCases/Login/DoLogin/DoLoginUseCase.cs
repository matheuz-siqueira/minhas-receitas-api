using MinhasReceitasApp.Application.Services.Cryptography;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Repositories.User;
using MinhasReceitasApp.Domain.Security.Tokens;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace MinhasReceitasApp.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository; 
    private readonly PasswordEncripter _passwordEncripter; 
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    public DoLoginUseCase(
        IUserReadOnlyRepository repository, 
        PasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _repository = repository; 
        _passwordEncripter = passwordEncripter; 
        _accessTokenGenerator = accessTokenGenerator;
    }
    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var encriptedPassword = _passwordEncripter.Encrypt(request.Password);
        var user = await _repository.GetByEmailAndPassword(request.Email, encriptedPassword) ?? throw new InvalidLoginException();
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
