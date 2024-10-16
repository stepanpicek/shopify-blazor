
using Microsoft.JSInterop;
using ShopifyApp.Core.Settings;

namespace ShopifyApp.Core.Services;

public interface IAppBridgeService
{
    Task<IJSObjectReference> GetOrCreateAppBridgeAsync(AppBridgeSettings settings);
    Task<string> GetSessionToken();
    bool IsAppBridgeInitialized();
}