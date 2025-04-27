using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using MinhasReceitasApp.Infrastructure.Security.Tokens.Access.Generator;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest : MinhasReceitasAppClassFixture
{
    private readonly string _method = "user"; 
    private readonly string _name; 
    private readonly string _email; 
    private readonly Guid _userIdentifier; 
    public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _name = factory.GetName();
        _email = factory.GetEmail();
        _userIdentifier = factory.GetUserIdentifier();
    }
    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier); 
        var response = await DoGet(_method, token: token); 

        response.StatusCode.Should().Be(HttpStatusCode.OK); 

        await using var responseBody = await response.Content.ReadAsStreamAsync(); 

        var responseData = await JsonDocument.ParseAsync(responseBody); 

        responseData.RootElement.GetProperty("name")
            .GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name); 
        
        responseData.RootElement.GetProperty("email")
            .GetString().Should().NotBeNullOrWhiteSpace().And.Be(_email); 
    }
}
