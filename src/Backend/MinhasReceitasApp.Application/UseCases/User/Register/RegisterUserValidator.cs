using FluentValidation;
using MinhasReceitasApp.Communication.Requests;


namespace MinhasReceitasApp.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("O nome não pode ser vazio.");
        RuleFor(user => user.Email).NotEmpty().WithMessage("O Email não pode ser vazio.");
        RuleFor(user => user.Email).EmailAddress().WithMessage("O Email deve ser válido."); 
        RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6).WithMessage("A senha deve ter pelo menos 6 caracteres."); 
    }
}
