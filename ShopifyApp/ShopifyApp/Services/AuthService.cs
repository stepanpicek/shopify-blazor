using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using ShopifyApp.Core.Services;
using ShopifyApp.Entities;
using ShopifyApp.Settings;
using ShopifySharp.Utilities;

namespace ShopifyApp.Services;

public class AuthService : IAuthService
{
    private readonly IShopifyDomainUtility _shopifyDomainUtility;
    private readonly IShopifyOauthUtility _shopifyOauthUtility;
    private readonly IShopifyRequestValidationUtility _shopifyRequestValidationUtility;
    private readonly UserManager<ShopifyUser> _userManager;
    private readonly ShopifySettings _shopifySettings;
    private const string SHOPIFY = "Shopify";
    private const string STATE_TOKEN_NAME = $"{SHOPIFY}_State";
    private const string AUTH_TOKEN_NAME = $"{SHOPIFY}_Auth";

    public AuthService(IShopifyDomainUtility shopifyDomainUtility, IShopifyOauthUtility shopifyOauthUtility, UserManager<ShopifyUser> userManager, ShopifySettings shopifySettings, IShopifyRequestValidationUtility shopifyRequestValidationUtility)
    {
        _shopifyDomainUtility = shopifyDomainUtility;
        _shopifyOauthUtility = shopifyOauthUtility;
        _userManager = userManager;
        _shopifySettings = shopifySettings;
        _shopifyRequestValidationUtility = shopifyRequestValidationUtility;
    }

    public Task<bool> IsValidShopifyRequestAsync(IDictionary<string,string> query)
    {
        return Task.FromResult(_shopifyRequestValidationUtility.IsAuthenticRequest( query, _shopifySettings.ClientSecret));
    }

    public async Task<bool> IsShopAuthenticated(string shop)
    {
        var user = await _userManager.FindByNameAsync(shop);
        if (user == null)
        {
            return false;
        }
        
        var token = await _userManager.GetAuthenticationTokenAsync(user, SHOPIFY, AUTH_TOKEN_NAME);
        return token != null;
    }

    public async Task AuthenticateShopAsync(string shop, string code, string state)
    {
        var user = await _userManager.FindByNameAsync(shop);
        if (user == null)
        {
            throw new AuthenticationException();
        }
        
        var token = await _userManager.GetAuthenticationTokenAsync(user, SHOPIFY, STATE_TOKEN_NAME);  
        if(token != state)
        {
            throw new AuthenticationException();
        }
        
        await _userManager.RemoveAuthenticationTokenAsync(user, SHOPIFY, STATE_TOKEN_NAME);
        
        var accessToken =
            await _shopifyOauthUtility.AuthorizeAsync(code, shop, _shopifySettings.ClientId, _shopifySettings.ClientSecret);
        
        await _userManager.SetAuthenticationTokenAsync(user, SHOPIFY, AUTH_TOKEN_NAME, accessToken.AccessToken);
    }

    public async Task<string> GetAuthUrlAsync(string shop)
    {
        if (!await _shopifyDomainUtility.IsValidShopDomainAsync(shop))
        {
            throw new InvalidOperationException("Invalid shop domain");
        }

        var user = await _userManager.FindByNameAsync(shop);
        if(user == null)
        {
            await _userManager.CreateAsync(new ShopifyUser { UserName = shop });
            user = await _userManager.FindByNameAsync(shop);
        }
        
        var state = Guid.NewGuid().ToString();
        await _userManager.SetAuthenticationTokenAsync(user!, SHOPIFY, STATE_TOKEN_NAME, state);
        
        var authorizationUrl = _shopifyOauthUtility.BuildAuthorizationUrl(_shopifySettings.Scopes, shop, _shopifySettings.ClientId, "https://patient-snail-moved.ngrok-free.app/api/shopify/callback", state);
        return authorizationUrl.ToString();
    }
}