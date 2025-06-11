using FluentValidation;
using MinhasReceitasApp.Application.SharedValidators;
using MinhasReceitasApp.Communication.Requests;

namespace MinhasReceitasApp.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}
