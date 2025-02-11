namespace ShopifyApp.Core.Endpoints;

public static class ShopifyEndpoints
{
    public const string ApiBase = "/api/shopify";
    public const string IsShopAuthenticated = "/is-shop-auth";
    public const string IsValidShopifyRequest = "/is-valid-shopify-request";
    public const string Authentication = "/authenticate";
    
    public const string WebhookBase = "/webhook";
    public const string Uninstall = "/uninstall";
    
    public const string ShopBase = "/api/shop";
    public const string GetShopInfo = "/info";
    
}