using System.Net;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.User.Profile;

public class GetUserProfileInvalidTokenTest : MinhasReceitasAppClassFixture
{
    private readonly string _method = "user";
    public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    { }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(method: _method, token: "invalidToken");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGet(method: _method, token: string.Empty);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        var response = await DoGet(method: _method, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
