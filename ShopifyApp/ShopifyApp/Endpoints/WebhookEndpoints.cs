using System.Text;
using Carter;
using Microsoft.AspNetCore.Mvc;
using ShopifyApp.Core.Dto.Webhooks;
using ShopifyApp.Core.Services;

namespace ShopifyApp.Endpoints;

public class WebhookEndpoints: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(Core.Endpoints.ShopifyEndpoints.WebhookBase);
        group.MapPost(Core.Endpoints.ShopifyEndpoints.Uninstall, UninstallAsync);
    }

    private async Task<IResult> UninstallAsync(
        [FromBody] UninstallWebhook request,
        [FromHeader(Name="X-Shopify-Shop-Domain")] string shopDomain,
        [FromServices] IAuthService authService,
        [FromServices] ILogger<WebhookEndpoints> logger)
    {
        logger.LogInformation("Uninstall webhook received: {@body}", request);
        await authService.RemoveShopAsync(request.Domain);
        return Results.Accepted();
    }
}