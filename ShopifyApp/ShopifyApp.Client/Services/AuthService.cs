using System.Net.Http.Json;
using ShopifyApp.Core.Endpoints;
using ShopifyApp.Core.Services;

namespace ShopifyApp.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsValidShopifyRequestAsync(IDictionary<string, string> query)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ShopifyEndpoints.ApiBase}{ShopifyEndpoints.IsValidShopifyRequest}", query);
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<bool> IsShopAuthenticatedAsync(string shop)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ShopifyEndpoints.ApiBase}{ShopifyEndpoints.IsShopAuthenticated}", shop);
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task AuthenticateShopAsync(string shop, string sessionToken)
    {
        var response = await _httpClient.PostAsJsonAsync($"{ShopifyEndpoints.ApiBase}{ShopifyEndpoints.Authentication}", shop);
        response.EnsureSuccessStatusCode();
    }

    public Task RemoveShopAsync(string shop)
    {
        throw new NotImplementedException();
    }
}