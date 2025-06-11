using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.User;
using MinhasReceitasApp.Domain.Security.Tokens;

namespace MinhasReceitasApp.Application.UseCases.Login.External;

public class ExternalLoginUseCase : IExternalLoginUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IAccessTokenGenerator _tokenGenerator;
    public ExternalLoginUseCase(
        IUserReadOnlyRepository readOnlyRepository,
        IUserWriteOnlyRepository writeOnlyRepository,
        IUnityOfWork unityOfWork,
        IAccessTokenGenerator tokenGenerator)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _unityOfWork = unityOfWork;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<string> Execute(string name, string email)
    {
        var user = await _readOnlyRepository.GetByEmail(email);

        if (user is null)
        {
            user = new Domain.Entities.User
            {
                Name = name,
                Email = email,
                Password = "-"
            };

            await _writeOnlyRepository.Add(user);
            await _unityOfWork.Commit();
        }
        return _tokenGenerator.Generate(user.UserIdentifier);
    }
}
