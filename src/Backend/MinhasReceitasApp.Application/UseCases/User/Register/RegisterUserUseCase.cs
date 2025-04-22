using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MinhasReceitasApp.Application.Services.AutoMapper;
using MinhasReceitasApp.Application.Services.Cryptography;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.User;
using MinhasReceitasApp.Exceptions.ExceptionsBase;

namespace MinhasReceitasApp.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository; 
    private readonly IMapper _mapper; 
    private readonly PasswordEncripter _passwordEncripter;
    private readonly IUnityOfWork _unityOfWork; 

    public RegisterUserUseCase(
        IUserReadOnlyRepository readOnlyRepository, 
        IUserWriteOnlyRepository writeOnlyRepository,
        IMapper mapper, 
        PasswordEncripter passwordEncripter, 
        IUnityOfWork unityOfWork)
    {
        _readOnlyRepository = readOnlyRepository; 
        _writeOnlyRepository = writeOnlyRepository; 
        _mapper = mapper; 
        _passwordEncripter = passwordEncripter; 
        _unityOfWork = unityOfWork;     
    }
    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request) 
    {
        await Validate(request); 
        var user = _mapper.Map<Domain.Entities.User>(request); 

        user.Password = _passwordEncripter.Encrypt(request.Password); 

        await _writeOnlyRepository.Add(user); 
        await _unityOfWork.Commit(); 

        return new ResponseRegisterUserJson{
            Name = user.Name, 
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
       var validator = new RegisterUserValidator(); 
       var result = validator.Validate(request);

       var emailExist = await _readOnlyRepository.ExistActiveWithEmail(request.Email); 
       if(emailExist)
            result.Errors.Add(new ValidationFailure(string.Empty, "E-mail já registrado."));
       

       if(!result.IsValid)
       {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList(); 
            throw new ErrorOnValidationException(errorMessages);
       } 
    }
}
