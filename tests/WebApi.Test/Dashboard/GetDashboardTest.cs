using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.Dashboard;

public class GetDashboardTest : MinhasReceitasAppClassFixture
{
    private const string METHOD = "dashboard";
    private readonly Guid _userIdentifier;
    public GetDashboardTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(method: METHOD, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").GetArrayLength().Should().BeGreaterThan(0);
    }

}
