using Carter;
using Microsoft.AspNetCore.Mvc;
using ShopifyApp.Settings;
using ShopifySharp.Utilities;

namespace ShopifyApp.Endpoints;

public class ShopifyEndpoints : ICarterModule

{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/shopify");
        group.MapGet("/install", Install);
        group.MapGet("/callback", Callback);
    }
    
    public static async Task<IResult> Install(
        [FromQuery]string shop,
        [FromServices] IShopifyDomainUtility shopifyDomainUtility,
        [FromServices] IShopifyOauthUtility shopifyOauthUtility,
        [FromServices] IConfiguration configuration)
    {
        if (!await shopifyDomainUtility.IsValidShopDomainAsync(shop))
        {
            return Results.Forbid();
        }

        var shopifyConfig = configuration.GetSection("Shopify").Get<ShopifySettings>() ?? throw new InvalidOperationException();
        var requiredPermissions = new [] { "read_orders" };
        var authorizationUrl = shopifyOauthUtility.BuildAuthorizationUrl(requiredPermissions, shop, shopifyConfig.ClientId, "https://patient-snail-moved.ngrok-free.app/api/shopify/callback", Guid.NewGuid().ToString());
        
        return Results.Redirect(authorizationUrl.ToString());
    }
    
    public static async Task<IResult> Callback(
        [FromQuery] string shop)
    {
        return Results.Redirect($"/shopify?shop={shop}");
    }
}