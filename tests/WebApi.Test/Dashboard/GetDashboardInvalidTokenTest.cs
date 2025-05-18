using System.Net;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.Dashboard;

public class GetDashboardInvalidTokenTest : MinhasReceitasAppClassFixture
{
    private const string METHOD = "dashboard";
    public GetDashboardInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    { }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(method: METHOD, token: "invalidToken");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGet(method: METHOD, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoGet(method: METHOD, token: token);
    }
}
