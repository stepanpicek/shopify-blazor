using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopifyApp.Core.Settings;

namespace ShopifyApp.Core.Services;

public class AppBridgeService(IJSRuntime jsRuntime, INavigationWrapper navigationManager) : IAppBridgeService
{
    private IJSObjectReference? _appBridge;
    
    public async Task CreateAppBridgeAsync(AppBridgeSettings settings)
    {
        _appBridge ??= await jsRuntime.InvokeAsync<IJSObjectReference>("appBridge.getAppBridge");
        await jsRuntime.InvokeVoidAsync("appBridge.subscribeNavigation", DotNetObjectReference.Create(navigationManager));
    }

    public async Task<string> GetSessionToken()
    {
        _ = _appBridge ?? throw new InvalidOperationException("AppBridge is not initialized");
        return await jsRuntime.InvokeAsync<string>("appBridge.getNewSessionToken");
    }

    public async Task<(bool isSuccess, string? sessionToken)> TryGetSessionToken()
    {
        if(_appBridge == null)
        {
            return (false, null);
        }

        try
        {
            var sessionToken = await jsRuntime.InvokeAsync<string>("appBridge.getNewSessionToken");
            return (true, sessionToken);
        }
        catch (Exception)
        {
            return (false, null);
        }
    }

    public bool IsAppBridgeInitialized()
    {
        return _appBridge != null;
    }

    public async Task NavigateAsync(string path)
    {
        await jsRuntime.InvokeVoidAsync("appBridge.redirect", path);
    }
}