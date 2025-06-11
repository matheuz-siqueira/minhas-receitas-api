using FluentValidation;
using MinhasReceitasApp.Application.SharedValidators;
using MinhasReceitasApp.Communication.Requests;


namespace MinhasReceitasApp.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("O nome não pode ser vazio.");
        RuleFor(user => user.Email).NotEmpty().WithMessage("O Email não pode ser vazio.");
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage("O Email deve ser válido.");
        });
    }
}
