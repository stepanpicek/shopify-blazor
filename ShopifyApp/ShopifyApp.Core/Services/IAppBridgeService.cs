using ShopifyApp.Core.Settings;

namespace ShopifyApp.Core.Services;

public interface IAppBridgeService
{
    Task CreateAppBridgeAsync(AppBridgeSettings settings);
    Task<string> GetSessionToken();
    Task<(bool isSuccess, string? sessionToken)> TryGetSessionToken();
    bool IsAppBridgeInitialized();
    Task NavigateAsync(string path);
}