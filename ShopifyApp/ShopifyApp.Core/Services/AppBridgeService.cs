using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ShopifyApp.Core.Settings;

namespace ShopifyApp.Core.Services;

public class AppBridgeService(IJSRuntime jsRuntime, ILogger<AppBridgeService> logger) : IAppBridgeService
{
    private IJSObjectReference? _appBridge;

    public async Task CreateAppBridgeAsync(AppBridgeSettings settings)
    {
        if (settings?.Host == null)
        {
            return;
        }

        try
        {
            _appBridge ??= await jsRuntime.InvokeAsync<IJSObjectReference>("appBridge.createAppBridge", settings);
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    public async Task<string> GetSessionToken()
    {
        var appBridge = _appBridge ?? throw new InvalidOperationException("AppBridge is not initialized");
        return await jsRuntime.InvokeAsync<string>("appBridge.getNewSessionToken", appBridge);
    }

    public async Task<(bool isSuccess, string? sessionToken)> TryGetSessionToken()
    {
        if(_appBridge == null)
        {
            return (false, null);
        }

        try
        {
            var sessionToken = await jsRuntime.InvokeAsync<string>("appBridge.getNewSessionToken", _appBridge);
            return (true, sessionToken);
        }
        catch (Exception e)
        {
            return (false, null);
        }
    }

    public bool IsAppBridgeInitialized()
    {
        return _appBridge != null;
    }
}