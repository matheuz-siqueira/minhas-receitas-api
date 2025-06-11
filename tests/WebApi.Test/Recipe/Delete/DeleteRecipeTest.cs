using System.Net;
using System.Text.Json;
using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using MinhasReceitasApp.Infrastructure.Security.Tokens.Access.Generator;

namespace WebApi.Test.Recipe.Delete;

public class DeleteRecipeTest : MinhasReceitasAppClassFixture
{
    private const string METHOD = "recipe";
    private readonly Guid _userIdentifier;
    private readonly string _recipeId;
    public DeleteRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _recipeId = factory.GetRecipeId();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoDelete(method: $"{METHOD}/{_recipeId}", token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        response = await DoGet(method: $"{METHOD}/{_recipeId}", token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Recipe_Not_Found()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var id = IdEncripterBuilder.Build().Encode(1000);

        var response = await DoDelete(method: $"{METHOD}/{id}", token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals("Recipe not found."));
    }
}
