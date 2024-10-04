using Gyminize.Contracts.Services;
using Gyminize.ViewModels;

using Microsoft.UI.Xaml;

namespace Gyminize.Activation;

// Trình xử lý kích hoạt mặc định, triển khai từ ActivationHandler<LaunchActivatedEventArgs>
public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // Không có ActivationHandler nào đã xử lý kích hoạt.
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(HomeViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}