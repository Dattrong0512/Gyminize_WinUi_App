// Tệp này định nghĩa dịch vụ kích hoạt ứng dụng.
// Dịch vụ này chịu trách nhiệm xử lý các sự kiện kích hoạt của ứng dụng.

using Gyminize.Activation;
using Gyminize.Contracts.Services;
using Gyminize.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IThemeSelectorService _themeSelectorService;
    private UIElement? _shell = null;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, IThemeSelectorService themeSelectorService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        // Thực hiện các tác vụ trước khi kích hoạt.
        await InitializeAsync();

        // Đặt nội dung cho MainWindow.
        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<GuidePage1>();
            App.MainWindow.Content = _shell ?? new Frame();
        }

        // Xử lý kích hoạt thông qua các ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        // Kích hoạt MainWindow.
        App.MainWindow.Activate();

        // Thực hiện các tác vụ sau khi kích hoạt.
        await StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        // Tìm ActivationHandler có thể xử lý activationArgs.
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            // Xử lý activationArgs bằng ActivationHandler tìm được.
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            // Xử lý activationArgs bằng defaultHandler nếu không có ActivationHandler nào khác có thể xử lý.
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        // Khởi tạo dịch vụ chọn chủ đề.
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        // Đặt chủ đề yêu cầu cho ứng dụng.
        await _themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
    }
}