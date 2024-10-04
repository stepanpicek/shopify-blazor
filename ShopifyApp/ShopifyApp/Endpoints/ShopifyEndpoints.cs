using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopifyApp.Entities;
using ShopifyApp.Filters;
using ShopifyApp.Settings;
using ShopifySharp;
using ShopifySharp.Utilities;

namespace ShopifyApp.Endpoints;

public class ShopifyEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/shopify")
            .AddEndpointFilter<ShopifyRequestFilter>();
        group.MapGet("/install", Install);
        group.MapGet("/callback", Callback);
        group.MapGet("/is-authenticated", IsAuthenticated);
    }

    private static async Task<IResult> Install(
        [FromQuery] string shop,
        [FromServices] IShopifyDomainUtility shopifyDomainUtility,
        [FromServices] IShopifyOauthUtility shopifyOauthUtility,
        [FromServices] ShopifySettings shopifyConfig,
        [FromServices] UserManager<ShopifyUser> userManager,
        HttpRequest request)
    {
        if (!await shopifyDomainUtility.IsValidShopDomainAsync(shop))
        {
            return Results.Forbid();
        }

        ShopifyUser? user = await userManager.FindByNameAsync(shop);
        if(user == null)
        {
            await userManager.CreateAsync(new ShopifyUser { UserName = shop });
            user = await userManager.FindByNameAsync(shop);
        }
        var state = Guid.NewGuid().ToString();
        await userManager.SetAuthenticationTokenAsync(user!, "Shopify", "Shopify", state);
        
        var requiredPermissions = new [] { "read_orders" };
        var authorizationUrl = shopifyOauthUtility.BuildAuthorizationUrl(requiredPermissions, shop, shopifyConfig.ClientId, "https://patient-snail-moved.ngrok-free.app/api/shopify/callback", state);
        
        return Results.Redirect(authorizationUrl.ToString());
    }

    private static async Task<IResult> Callback(
        [FromQuery] string shop,
        [FromQuery] string host,
        [FromQuery] string hmac,
        [FromQuery] string state,
        [FromQuery] string code,
        [FromServices] ShopifySettings shopifyConfig,
        [FromServices] UserManager<ShopifyUser> userManager,
        [FromServices] IShopifyOauthUtility shopifyOauthUtility,
        HttpRequest request)
    {
        ShopifyUser? user = await userManager.FindByNameAsync(shop);
        if (user == null)
        {
            return Results.Forbid();
        }
        
        var token = await userManager.GetAuthenticationTokenAsync(user, "Shopify", "Shopify");  
        if(token != state)
        {
            return Results.Forbid();
        }
        
        await userManager.RemoveAuthenticationTokenAsync(user, "Shopify", "Shopify");
        
        var accessToken =
            await shopifyOauthUtility.AuthorizeAsync(code, shop, shopifyConfig.ClientId, shopifyConfig.ClientSecret);
        
        await userManager.SetAuthenticationTokenAsync(user, "Shopify", "ShopifyAuth", accessToken.AccessToken);
        
        return Results.Redirect("/shopify?host=" + host);
    }

    private static async Task<IResult> IsAuthenticated(string code, [FromServices] UserManager<ShopifyUser> userManager)
    {
        return Results.Forbid();
    }
}