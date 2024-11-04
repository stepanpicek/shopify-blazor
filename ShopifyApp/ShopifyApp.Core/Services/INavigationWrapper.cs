using Microsoft.JSInterop;

namespace ShopifyApp.Core.Services;

public interface INavigationWrapper
{
    [JSInvokable]
    void NavigateTo(string path);
}