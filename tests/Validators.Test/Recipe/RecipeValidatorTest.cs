using CommonTestUtilities.Requests;
using FluentAssertions;
using MinhasReceitasApp.Application.UseCases.Recipe;
using MinhasReceitasApp.Communication.Enums;

namespace Validators.Test.Recipe;

public class RecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Invalid_CookingTime()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.CookingTime = (CookingTime?)1000;

        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Unsupported cooking time."));
    }

    [Fact]
    public void Error_Invalid_Difficulity()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.Difficulty = (Difficulty?)1000;

        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Unsupported difficulty."));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("          ")]
    [InlineData("")]
    public void Error_Empty_Title(string title)
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = title;
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("title cannot be empty."));
    }

    [Fact]
    public void Success_CookingTime_Null()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.CookingTime = null;
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Success_Difficulty_Null()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Difficulty = null;
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Success_DishTypes_Empty()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Clear();
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Invalid_DishTypes()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Add((DishType)1000);
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("unsupported dish type."));

    }

    [Fact]
    public void Error_Empty_Ingredients()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Clear();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("recipe must have at least one ingredient."));
    }

    [Fact]
    public void Error_Empty_Instructions()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.Clear();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("recipe must have at least one instruction."));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("        ")]
    [InlineData("")]
    public void Error_Empty_Invalid_Values_Ingredients(string ingredient)
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients.Add(ingredient);
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("ingredients cannot be empty."));
    }

    [Fact]
    public void Error_Same_Step_Instructions()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Step = request.Instructions.Last().Step;
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("two or more instructions have the same order."));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Error_Negative_Step_Instructions(int step)
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Step = step;
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("step cannot be negative or zero."));
    }

    [Theory]
    [InlineData("        ")]
    [InlineData("")]
    [InlineData(null)]
    public void Error_Empty_Values_Instructions(string instruction)
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Text = instruction;
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("instruction text cannot be empty."));
    }

    [Fact]
    public void Error_Instructions_Too_Long()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Text = RequestStringGenerator.Paragraphs(minCharacters: 2001);

        var validator = new RecipeValidator();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("instruction text must have a maximum of 2000 characters."));
    }
}
