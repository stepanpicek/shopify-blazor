using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ShopifyApp.Core.Settings;

namespace ShopifyApp.Core.Services;

public class AppBridgeService(IJSRuntime jsRuntime, ILogger<AppBridgeService> logger) : IAppBridgeService
{
    private IJSObjectReference? _appBridge = null;

    public async Task<IJSObjectReference> GetOrCreateAppBridgeAsync(AppBridgeSettings settings)
    {
        return _appBridge ??= await jsRuntime.InvokeAsync<IJSObjectReference>("appBridge.createAppBridge", settings);
    }

    public async Task<string> GetSessionToken()
    {
        var appBridge = _appBridge ?? throw new InvalidOperationException("AppBridge is not initialized");
        return await jsRuntime.InvokeAsync<string>("appBridge.getNewSessionToken", appBridge);
    }

    public bool IsAppBridgeInitialized()
    {
        return _appBridge != null;
    }
}