using FluentValidation;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Domain.Extensions;

namespace MinhasReceitasApp.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage("O nome não pode ser vazio.");
        RuleFor(request => request.Email).NotEmpty().WithMessage("O Email não pode ser vazio.");

        When(request => string.IsNullOrWhiteSpace(request.Email).IsFalse(), () =>
        {
            RuleFor(request => request.Email).EmailAddress().WithMessage("O Email deve ser válido.");
        });
    }
}
