using FluentValidation;
using MinhasReceitasApp.Communication.Requests;

namespace MinhasReceitasApp.Application.UseCases.Recipe.Filter;

public class FilterRecipeValidator : AbstractValidator<RequestFilterRecipeJson>
{
    public FilterRecipeValidator()
    {
        RuleForEach(r => r.CookingTimes).IsInEnum().WithMessage("Unsupported cooking time.");
        RuleForEach(r => r.Difficulties).IsInEnum().WithMessage("Unsupported difficulty.");
        RuleForEach(r => r.DishTypes).IsInEnum().WithMessage("unsupported dish type.");
    }
}
