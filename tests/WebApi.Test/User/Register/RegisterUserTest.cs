using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly string method = "user"; 
    private readonly HttpClient _httpClient;
    public RegisterUserTest(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient(); 
    
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var response = await _httpClient.PostAsJsonAsync(method, request); 

        response.StatusCode.Should().Be(HttpStatusCode.Created); 

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody); 

        responseData.RootElement.GetProperty("name")
            .GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Name); 

    }

    [Fact]
    public async Task Error_Empty_Name()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty; 

        var response = await _httpClient.PostAsJsonAsync(method, request); 

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody); 

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray(); 

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals("O nome não pode ser vazio.")); 
    }
}
