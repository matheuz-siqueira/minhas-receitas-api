using System.Net;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.Recipe.Filter;

public class FilterRecipeInvalidTokenTest : MinhasReceitasAppClassFixture
{
    private const string METHOD = "recipe/filter";
    public FilterRecipeInvalidTokenTest(CustomWebApplicationFactory webApplication) : base(webApplication)
    { }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();

        var response = await DoPost(method: METHOD, request: request, token: "invalidToken");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();

        var response = await DoPost(method: METHOD, request: request, token: string.Empty);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPost(method: METHOD, request: request, token: token);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
