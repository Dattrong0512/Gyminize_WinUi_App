using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;

using Microsoft.Web.WebView2.Core;

namespace Gyminize.ViewModels;

// TODO: Review best practices and distribution guidelines for WebView2.
// https://docs.microsoft.com/microsoft-edge/webview2/get-started/winui
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/developer-guide
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/distribution
public partial class WebViewViewModel : ObservableRecipient, INavigationAware
{
    // TODO: Set the default URL to display.
    [ObservableProperty]
    private Uri source = new("https://docs.microsoft.com/windows/apps/");

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private bool hasFailures;

    public IWebViewService WebViewService
    {
        get;
    }

    private ILocalSettingsService _localSettingsService;

    public WebViewViewModel(IWebViewService webViewService, ILocalSettingsService localSettingsService)
    {
        WebViewService = webViewService;
        _localSettingsService = localSettingsService;
        //InitializeAsync();
        
    }

    public async Task InitializeAsync()
    {
        var link = await _localSettingsService.ReadSettingAsync<string>("YtbLink");
        if (!string.IsNullOrEmpty(link))
        {
            Source = new Uri(link);
        }
    }

    [RelayCommand]
    private async Task OpenInBrowser()
    {
        if (WebViewService.Source != null)
        {
            await Windows.System.Launcher.LaunchUriAsync(WebViewService.Source);
        }
    }

    [RelayCommand]
    private void Reload()
    {
        WebViewService.Reload();
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoForward))]
    private void BrowserForward()
    {
        if (WebViewService.CanGoForward)
        {
            WebViewService.GoForward();
        }
    }

    private bool BrowserCanGoForward()
    {
        return WebViewService.CanGoForward;
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoBack))]
    private void BrowserBack()
    {
        if (WebViewService.CanGoBack)
        {
            WebViewService.GoBack();
        }
    }

    private bool BrowserCanGoBack()
    {
        return WebViewService.CanGoBack;
    }

    public void OnNavigatedTo(object parameter)
    {
        if(parameter is string)
        {
            var link = parameter as string;
            Source = new Uri(link);
        }
        WebViewService.NavigationCompleted += OnNavigationCompleted;
    }

    public void OnNavigatedFrom()
    {
        StopWebView();
        WebViewService.UnregisterEvents();
        WebViewService.NavigationCompleted -= OnNavigationCompleted;
    }

    private void OnNavigationCompleted(object? sender, CoreWebView2WebErrorStatus webErrorStatus)
    {
        IsLoading = false;
        BrowserBackCommand.NotifyCanExecuteChanged();
        BrowserForwardCommand.NotifyCanExecuteChanged();

        if (webErrorStatus != default)
        {
            HasFailures = true;
        }
    }

    [RelayCommand]
    private void OnRetry()
    {
        HasFailures = false;
        IsLoading = true;
        WebViewService?.Reload();
    }
    private void StopWebView()
    {
        // Navigate to a blank page to stop any media playback
        Source = new Uri("about:blank");
    }
}
