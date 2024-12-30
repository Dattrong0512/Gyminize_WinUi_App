using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;

using Microsoft.Web.WebView2.Core;

namespace Gyminize.ViewModels;

/// <summary>
/// ViewModel cho WebView, cung cấp các thuộc tính và lệnh để điều khiển WebView.
/// </summary>
public partial class WebViewViewModel : ObservableRecipient, INavigationAware
{
    /// <summary>
    /// Nguồn của WebView.
    /// </summary>
    [ObservableProperty]
    private Uri source = new("https://docs.microsoft.com/windows/apps/");

    /// <summary>
    /// Trạng thái tải của WebView.
    /// </summary>
    [ObservableProperty]
    private bool isLoading = true;

    /// <summary>
    /// Trạng thái lỗi của WebView.
    /// </summary>
    [ObservableProperty]
    private bool hasFailures;

    /// <summary>
    /// Dịch vụ WebView.
    /// </summary>
    public IWebViewService WebViewService
    {
        get;
    }

    private ILocalSettingsService _localSettingsService;

    /// <summary>
    /// Khởi tạo một thể hiện mới của lớp <see cref="WebViewViewModel"/>.
    /// </summary>
    /// <param name="webViewService">Dịch vụ WebView.</param>
    /// <param name="localSettingsService">Dịch vụ cài đặt cục bộ.</param>
    public WebViewViewModel(IWebViewService webViewService, ILocalSettingsService localSettingsService)
    {
        WebViewService = webViewService;
        _localSettingsService = localSettingsService;
    }

    /// <summary>
    /// Khởi tạo ViewModel, đọc cài đặt liên kết từ dịch vụ cài đặt cục bộ.
    /// </summary>
    /// <returns>Một tác vụ đại diện cho thao tác không đồng bộ.</returns>
    public async Task InitializeAsync()
    {
        var link = await _localSettingsService.ReadSettingAsync<string>("YtbLink");
        if (!string.IsNullOrEmpty(link))
        {
            Source = new Uri(link);
        }
    }

    /// <summary>
    /// Lệnh mở liên kết hiện tại trong trình duyệt mặc định.
    /// </summary>
    /// <returns>Một tác vụ đại diện cho thao tác không đồng bộ.</returns>
    [RelayCommand]
    private async Task OpenInBrowser()
    {
        if (WebViewService.Source != null)
        {
            await Windows.System.Launcher.LaunchUriAsync(WebViewService.Source);
        }
    }

    /// <summary>
    /// Lệnh tải lại WebView.
    /// </summary>
    [RelayCommand]
    private void Reload()
    {
        WebViewService.Reload();
    }

    /// <summary>
    /// Lệnh điều hướng tới trang tiếp theo trong lịch sử duyệt web.
    /// </summary>
    [RelayCommand(CanExecute = nameof(BrowserCanGoForward))]
    private void BrowserForward()
    {
        if (WebViewService.CanGoForward)
        {
            WebViewService.GoForward();
        }
    }

    /// <summary>
    /// Kiểm tra xem WebView có thể điều hướng tới trang tiếp theo hay không.
    /// </summary>
    /// <returns>True nếu có thể điều hướng tới trang tiếp theo, ngược lại là False.</returns>
    private bool BrowserCanGoForward()
    {
        return WebViewService.CanGoForward;
    }

    /// <summary>
    /// Lệnh điều hướng tới trang trước đó trong lịch sử duyệt web.
    /// </summary>
    [RelayCommand(CanExecute = nameof(BrowserCanGoBack))]
    private void BrowserBack()
    {
        if (WebViewService.CanGoBack)
        {
            WebViewService.GoBack();
        }
    }

    /// <summary>
    /// Kiểm tra xem WebView có thể điều hướng tới trang trước đó hay không.
    /// </summary>
    /// <returns>True nếu có thể điều hướng tới trang trước đó, ngược lại là False.</returns>
    private bool BrowserCanGoBack()
    {
        return WebViewService.CanGoBack;
    }

    /// <summary>
    /// Phương thức được gọi khi điều hướng đến trang.
    /// </summary>
    /// <param name="parameter">Dữ liệu được truyền khi điều hướng.</param>
    public void OnNavigatedTo(object parameter)
    {
        if (parameter is string)
        {
            var link = parameter as string;
            Source = new Uri(link);
        }
        WebViewService.NavigationCompleted += OnNavigationCompleted;
    }

    /// <summary>
    /// Phương thức được gọi khi điều hướng đi từ trang.
    /// </summary>
    public void OnNavigatedFrom()
    {
        StopWebView();
        WebViewService.UnregisterEvents();
        WebViewService.NavigationCompleted -= OnNavigationCompleted;
    }

    /// <summary>
    /// Xử lý sự kiện hoàn thành điều hướng của WebView.
    /// </summary>
    /// <param name="sender">Nguồn của sự kiện.</param>
    /// <param name="webErrorStatus">Trạng thái lỗi của WebView.</param>
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

    /// <summary>
    /// Lệnh thử lại điều hướng khi có lỗi.
    /// </summary>
    [RelayCommand]
    private void OnRetry()
    {
        HasFailures = false;
        IsLoading = true;
        WebViewService?.Reload();
    }

    /// <summary>
    /// Dừng WebView bằng cách điều hướng đến trang trống.
    /// </summary>
    private void StopWebView()
    {
        // Navigate to a blank page to stop any media playback
        Source = new Uri("about:blank");
    }
}
