using System.Net.Http.Headers;
using ShopifyApp.Core.Services;

namespace ShopifyApp.Client.Handlers;

public class ShopifyAuthHandler(IAppBridgeService appBridgeService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await appBridgeService.TryGetSessionToken();

        if (token.isSuccess && !string.IsNullOrEmpty(token.sessionToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.sessionToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}