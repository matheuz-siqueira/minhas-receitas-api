using System.Net;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Microsoft.VisualBasic;

namespace WebApi.Test.User.Update;

public class UpdateUserInvalidTokenTest : MinhasReceitasAppClassFixture
{
    private readonly string _method = "user";
    public UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    { }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(method: _method, request: request, token: "INvalidToKen");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Withou_Token()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(method: _method, request: request, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(method: _method, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


}
