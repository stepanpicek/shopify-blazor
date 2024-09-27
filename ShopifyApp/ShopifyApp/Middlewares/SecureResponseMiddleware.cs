namespace ShopifyApp.Middlewares;

public class SecureResponseMiddleware
{
    private readonly RequestDelegate _next;

    public SecureResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var shop = context.Request.Query["shop"].ToString();
        context.Response.Headers.Append("Content-Security-Policy",
            $"frame-ancestors https://{shop} https://admin.shopify.com;");
        await _next(context);
    }
}