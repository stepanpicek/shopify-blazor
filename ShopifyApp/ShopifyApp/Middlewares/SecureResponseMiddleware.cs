namespace ShopifyApp.Middlewares;

public class SecureResponseMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var shop = context.Request.Query["shop"].ToString();
        context.Response.Headers.Append("Content-Security-Policy",
            $"frame-ancestors {shop} admin.shopify.com;");
        await next(context);
    }
}