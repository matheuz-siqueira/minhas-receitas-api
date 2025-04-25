using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MinhasReceitasApp.Communication.Requests;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : MinhasReceitasAppClassFixture
{
    private readonly string method = "login"; 
    private readonly string _email; 
    private readonly string _password; 
    private readonly string _name; 
    public DoLoginTest(CustomWebApplicationFactory factory) : base (factory)
    {
        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _name = factory.GetName();
    }

    [Fact]
    public async Task Success() 
    {
        var request = new RequestLoginJson
        {
            Email = _email, 
            Password = _password
        };

        var response = await DoPost(method, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync(); 

        var responseData = await JsonDocument.ParseAsync(responseBody); 

        responseData.RootElement
            .GetProperty("name").GetString().Should()
                .NotBeNullOrWhiteSpace().And.Be(_name);
    } 

    [Fact]
    public async Task Error_Login_Invalid()
    {
        var request = RequestLoginJsonBuilder.Build(); 

        var response = await DoPost(method, request); 
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync(); 

        var responseData = await JsonDocument.ParseAsync(responseBody); 

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray(); 

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals("Email and/or password invalid."));
    }
}
