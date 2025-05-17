using System.Net;
using System.Text.Json;
using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Microsoft.AspNetCore.RequestDecompression;

namespace WebApi.Test.Recipe.GetById;

public class GetRecipeByIdTest : MinhasReceitasAppClassFixture
{
    private const string METHOD = "recipe";
    private readonly Guid _userIdentifier;
    private readonly string _recipeId;
    private readonly string _recipeTitle;

    public GetRecipeByIdTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _recipeTitle = factory.GetRecipeTitle();
        _recipeId = factory.GetRecipeId();
    }

    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(method: $"{METHOD}/{_recipeId}", token: token);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().Should().Be(_recipeId);
        responseData.RootElement.GetProperty("title").GetString().Should().Be(_recipeTitle);
    }

    public async Task Error_Recipe_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var id = IdEncripterBuilder.Build().Encode(1987);

        var response = await DoGet(method: $"{METHOD}/{id}", token: token);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals("title cannot be empty."));
    }
}
