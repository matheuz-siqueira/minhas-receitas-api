using System.Net.Http.Headers;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.User;
using MinhasReceitasApp.Domain.Security.Cryptography;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace MinhasReceitasApp.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUnityOfWork _unityOfWork;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IPasswordEncripter passwordEncripter,
        IUnityOfWork unityOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _unityOfWork = unityOfWork;
    }
    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.User();
        Validate(request, loggedUser);

        var user = await _repository.GetById(loggedUser.Id);

        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _repository.Update(user);

        await _unityOfWork.Commit();
    }
    private void Validate(RequestChangePasswordJson request, Domain.Entities.User user)
    {
        var result = new ChangePasswordValidator().Validate(request);

        if (_passwordEncripter.IsValid(request.Password, user.Password).IsFalse())
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, "A senha atual está incorreta."));

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());

    }
}
