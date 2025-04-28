using FluentValidation;
using FluentValidation.Validators;

namespace MinhasReceitasApp.Application.SharedValidators;

public class PasswordValidator<T> : PropertyValidator<T, string>
{
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", "A senha não pode ser vazia.");
            return false;
        }

        if (password.Length < 6)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", "A senha deve ter pelo menos 6 caracteres.");
            return false;
        }
        return true;
    }

    public override string Name => "PasswordValidator";
    protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";
}
