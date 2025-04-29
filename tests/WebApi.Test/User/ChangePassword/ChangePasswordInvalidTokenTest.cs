using System.Net;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.User.ChangePassword;

public class ChangePasswordInvalidTokenTest : MinhasReceitasAppClassFixture
{
    private readonly string _method = "user/change-password";
    public ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    { }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var response = await DoPut(_method, request, token: "INvalidToKen");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Withou_Token()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var response = await DoPut(_method, request, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(_method, request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

}
