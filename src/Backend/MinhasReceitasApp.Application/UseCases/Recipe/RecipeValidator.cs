using FluentValidation;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Domain.Entities;

namespace MinhasReceitasApp.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title).NotEmpty().WithMessage("title cannot be empty.");
        RuleFor(recipe => recipe.CookingTime).IsInEnum().WithMessage("Unsupported cooking time.");
        RuleFor(recipe => recipe.Difficulity).IsInEnum().WithMessage("Unsupported difficulity.");
        RuleFor(recipe => recipe.Ingredients.Count).GreaterThan(0).WithMessage("recipe must have at least one ingredient.");
        RuleFor(recipe => recipe.Instructions.Count).GreaterThan(0).WithMessage("recipe must have at least one instruction.");
        RuleForEach(recipe => recipe.DishTypes).IsInEnum().WithMessage("unsupported dish type.");
        RuleForEach(recipe => recipe.Ingredients).NotEmpty().WithMessage("ingredients cannot be empty.");
        RuleForEach(recipe => recipe.Instructions).ChildRules(instructionRule =>
        {
            instructionRule.RuleFor(instruction => instruction.Step).GreaterThan(0).WithMessage("step cannot be negative.");
            instructionRule.RuleFor(instruction => instruction.Text)
                .NotEmpty().WithMessage("instruction text cannot be empty.")
                .MaximumLength(2000).WithMessage("instruction text must have a maximum of 2000 characters.");

        });

        RuleFor(recipe => recipe.Instructions)
            .Must(instructions => instructions.Select(i => i.Step).Distinct().Count() == instructions.Count)
                .WithMessage("two or more instructions have the same order.");

    }
}
