using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : MinhasReceitasAppClassFixture
{
    private readonly string method = "user"; 
    
    public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) {}

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var response = await DoPost(method, request); 

        response.StatusCode.Should().Be(HttpStatusCode.Created); 

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody); 

        responseData.RootElement.GetProperty("name")
            .GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Name); 
        responseData.RootElement
            .GetProperty("tokens").GetProperty("accessToken").GetString()
                .Should().NotBeNullOrEmpty(); 

    }

    [Fact]
    public async Task Error_Empty_Name()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty; 

        var response = await DoPost(method, request); 

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody); 

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray(); 

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals("O nome não pode ser vazio.")); 
    }
}
