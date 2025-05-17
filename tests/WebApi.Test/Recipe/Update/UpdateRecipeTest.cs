using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MinhasReceitasApp.Infrastructure.Security.Tokens.Access.Generator;

namespace WebApi.Test.Recipe.Update;

public class UpdateRecipeTest : MinhasReceitasAppClassFixture
{
    private const string METHOD = "recipe";
    private readonly Guid _userIdentifier;
    private readonly string _recipeId;
    public UpdateRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _recipeId = factory.GetRecipeId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(method: $"{METHOD}/{_recipeId}", request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoPut(method: $"{METHOD}/{_recipeId}", request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals("title cannot be empty."));
    }
}
