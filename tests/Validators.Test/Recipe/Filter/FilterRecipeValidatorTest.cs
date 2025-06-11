using CommonTestUtilities.Requests;
using FluentAssertions;
using MinhasReceitasApp.Application.UseCases.Recipe.Filter;

namespace Validators.Test.Recipe.Filter;

public class FilterRecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Invalid_Cooking_Time()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();

        request.CookingTimes.Add((MinhasReceitasApp.Communication.Enums.CookingTime)1000);
        var result = validator.Validate(request);


        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Unsupported cooking time."));
    }

    [Fact]
    public void Error_Invalid_Difficulty()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();

        request.Difficulties.Add((MinhasReceitasApp.Communication.Enums.Difficulty)1000);
        var result = validator.Validate(request);


        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Unsupported difficulty."));
    }

    [Fact]
    public void Error_Invalid_Dish_Types()
    {
        var validator = new FilterRecipeValidator();
        var request = RequestFilterRecipeJsonBuilder.Build();

        request.DishTypes.Add((MinhasReceitasApp.Communication.Enums.DishType)1000);
        var result = validator.Validate(request);


        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("unsupported dish type."));
    }

}
