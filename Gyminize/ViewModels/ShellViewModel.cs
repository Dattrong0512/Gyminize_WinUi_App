// ViewModel cho Shell (khung chính của ứng dụng).
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using CommunityToolkit.Mvvm.ComponentModel;

using Gyminize.Contracts.Services;
using Gyminize.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Gyminize.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    // Thuộc tính kiểm tra xem có thể quay lại trang trước hay không.
    [ObservableProperty]
    private bool isBackEnabled;

    // Thuộc tính lưu trữ mục được chọn trong NavigationView.
    [ObservableProperty]
    private object? selected;

    // Dịch vụ điều hướng.
    public INavigationService NavigationService
    {
        get;
    }

    // Dịch vụ NavigationView.
    public INavigationViewService NavigationViewService
    {
        get;
    }
    private Frame _frame;

    // Phương thức này sẽ được gọi từ ShellPage để gán Frame cho ViewModel
    public void SetFrame(Frame frame)
    {
        _frame = frame;
    }

    // Khởi tạo ShellViewModel với INavigationService và INavigationViewService.
    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    // Xử lý sự kiện điều hướng.
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;
        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}