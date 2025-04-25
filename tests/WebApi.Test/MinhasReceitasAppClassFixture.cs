using System.Net.Http.Json;

namespace WebApi.Test;

public class MinhasReceitasAppClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    public MinhasReceitasAppClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string method, object request)
    {
        return await _httpClient.PostAsJsonAsync(method, request); 
    }
}
