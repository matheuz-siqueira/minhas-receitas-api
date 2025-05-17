using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MinhasReceitasApp.Communication.Requests;

namespace WebApi.Test.Recipe.Filter;

public class FilterRecipeTest : MinhasReceitasAppClassFixture
{
    private const string METHOD = "recipe/filter";
    private readonly Guid _userIdentifier;
    private string _recipeTitle;
    private MinhasReceitasApp.Domain.Enums.Difficulty _recipeDifficultyLevel;
    private MinhasReceitasApp.Domain.Enums.CookingTime _recipeCookingTime;
    private IList<MinhasReceitasApp.Domain.Enums.DishType> _recipeDishTypes;
    public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)

    {
        _userIdentifier = factory.GetUserIdentifier();

        _recipeTitle = factory.GetRecipeTitle();
        _recipeCookingTime = factory.GetRecipeCookingTime();
        _recipeDifficultyLevel = factory.GetRecipeDifficulty();
        _recipeDishTypes = factory.GetRecipeDishTypes();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestFilterRecipeJson
        {
            CookingTimes = [(MinhasReceitasApp.Communication.Enums.CookingTime)_recipeCookingTime],
            Difficulties = [(MinhasReceitasApp.Communication.Enums.Difficulty)_recipeDifficultyLevel],
            DishTypes = _recipeDishTypes.Select(dishType => (MinhasReceitasApp.Communication.Enums.DishType)dishType).ToList(),
            RecipeTitle_Ingredient = _recipeTitle
        };

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").EnumerateArray().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.RecipeTitle_Ingredient = "recipeDontExists";

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_CookingTime_Invalid()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((MinhasReceitasApp.Communication.Enums.CookingTime)1000);

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals("Unsupported cooking time."));
    }
}
