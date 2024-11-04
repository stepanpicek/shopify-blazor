using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using ShopifyApp.Core.Services;
using ShopifyApp.Core.Settings;

namespace ShopifyApp.Core.Providers;

public class ShopifyAuthProvider : AuthenticationStateProvider
{
    private readonly IAuthService _authService;
    private readonly NavigationManager _navigation;
    private readonly IAppBridgeService _appBridgeService;
    private readonly ShopifySettings _shopifySettings;
    private readonly ILogger<ShopifyAuthProvider> _logger;

    public ShopifyAuthProvider(IAuthService authService, NavigationManager navigation, IAppBridgeService appBridgeService, ShopifySettings shopifySettings, ILogger<ShopifyAuthProvider> logger)
    {
        _authService = authService;
        _navigation = navigation;
        _appBridgeService = appBridgeService;
        _shopifySettings = shopifySettings;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var uriBuilder = new UriBuilder(_navigation.Uri);
        var q = HttpUtility.ParseQueryString(uriBuilder.Query);
        var host = q["host"];
        var shop= q["shop"];
        if (!await _authService.IsValidShopifyRequestAsync(q.Cast<string>().ToDictionary(s => s, s => q[s])))
        {
            _logger.LogWarning("Invalid Shopify request from {URI}", _navigation.Uri);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        try
        {
            await _appBridgeService.CreateAppBridgeAsync(new AppBridgeSettings
            {
                ApiKey = _shopifySettings.ClientId,
                Host = host
            });
        }
        catch (InvalidOperationException e)
        {
            //Prerender
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
            
        if (!await _authService.IsShopAuthenticatedAsync(shop))
        {
            var token = await _appBridgeService.GetSessionToken();
            await _authService.AuthenticateShopAsync(shop, token);
        }
        
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity("Shopify", "Shopify", "User")));
    }
}