using ShopifyApp.Settings;
using ShopifySharp.Utilities;

namespace ShopifyApp.Filters;

public class ShopifyRequestFilter(
    IShopifyRequestValidationUtility shopifyRequestValidationUtility,
    ShopifySettings settings,
    ILogger<ShopifyRequestFilter> logger)
    : IEndpointFilter
{

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        logger.LogInformation("ShopifyRequestFilter invoked {Query}", context.HttpContext.Request.Query);
        if (shopifyRequestValidationUtility.IsAuthenticRequest(context.HttpContext.Request.Query,
                settings.ClientSecret))
        {
            return await next(context);
        }

        return Results.Forbid();
    }
}