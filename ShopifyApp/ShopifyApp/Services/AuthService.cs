using Microsoft.AspNetCore.Identity;
using ShopifyApp.Core.Dto;
using ShopifyApp.Core.Services;
using ShopifyApp.Core.Settings;
using ShopifyApp.Entities;
using ShopifySharp.Utilities;

namespace ShopifyApp.Services;

public class AuthService : IAuthService
{
    private readonly IShopifyDomainUtility _shopifyDomainUtility;
    private readonly IShopifyOauthUtility _shopifyOauthUtility;
    private readonly IShopifyRequestValidationUtility _shopifyRequestValidationUtility;
    private readonly UserManager<ShopifyUser> _userManager;
    private readonly ShopifySettings _shopifySettings;
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthService> _logger;
    private const string SHOPIFY = "Shopify";
    private const string STATE_TOKEN_NAME = $"{SHOPIFY}_State";
    private const string AUTH_TOKEN_NAME = $"{SHOPIFY}_Auth";

    public AuthService(IShopifyDomainUtility shopifyDomainUtility, IShopifyOauthUtility shopifyOauthUtility, UserManager<ShopifyUser> userManager, ShopifySettings shopifySettings, IShopifyRequestValidationUtility shopifyRequestValidationUtility, HttpClient httpClient, ILogger<AuthService> logger)
    {
        _shopifyDomainUtility = shopifyDomainUtility;
        _shopifyOauthUtility = shopifyOauthUtility;
        _userManager = userManager;
        _shopifySettings = shopifySettings;
        _shopifyRequestValidationUtility = shopifyRequestValidationUtility;
        _httpClient = httpClient;
        _logger = logger;
    }

    public Task<bool> IsValidShopifyRequestAsync(IDictionary<string, string> query)
    {
        return Task.FromResult(_shopifyRequestValidationUtility.IsAuthenticRequest(query, _shopifySettings.ClientSecret));
    }

    public async Task<bool> IsShopAuthenticatedAsync(string shop)
    {
        var user = await _userManager.FindByNameAsync(shop);
        if (user == null)
        {
            return false;
        }
        
        var token = await _userManager.GetAuthenticationTokenAsync(user, SHOPIFY, AUTH_TOKEN_NAME);
        return token != null;
    }

    public async Task AuthenticateShopAsync(string shop, string sessionToken)
    {
        if (!await _shopifyDomainUtility.IsValidShopDomainAsync(shop))
        {
            throw new InvalidOperationException("Invalid shop domain");
        }

        var user = await _userManager.FindByNameAsync(shop);
        if (user == null)
        {
            await _userManager.CreateAsync(new ShopifyUser { UserName = shop });
            user = await _userManager.FindByNameAsync(shop);
        }

        var url = _shopifyDomainUtility.BuildShopDomainUri(shop);
        var accessToken = await GetShopifyAccessTokenAsync(url.ToString(), sessionToken);
        _logger.LogInformation($"Access token for {shop}: {accessToken}, session token: {sessionToken}");
        await _userManager.SetAuthenticationTokenAsync(user, SHOPIFY, AUTH_TOKEN_NAME, accessToken);
    }

    public async Task RemoveShopAsync(string shop)
    {
        var user = await _userManager.FindByNameAsync(shop);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
    }

    public async Task<string> GetShopAuthTokenAsync(string shop)
    {
        var user = await _userManager.FindByNameAsync(shop);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        return await _userManager.GetAuthenticationTokenAsync(user, SHOPIFY, AUTH_TOKEN_NAME);
    }

    private async Task<string?> GetShopifyAccessTokenAsync(string url, string sessionToken)
    {
        var request = new GetOfflineAccessTokenRequest(_shopifySettings.ClientId, _shopifySettings.ClientSecret, sessionToken);
        var response = await _httpClient.PostAsJsonAsync($"{url}admin/oauth/access_token", request);
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadFromJsonAsync<GetOfflineAccessTokenResponse>();
        return jsonResponse?.AccessToken;
    }
}