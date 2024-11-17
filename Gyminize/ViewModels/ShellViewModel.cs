// ViewModel cho Shell (khung chính của ứng dụng).
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using CommunityToolkit.Mvvm.ComponentModel;

using Gyminize.Contracts.Services;
using Gyminize.Core.Services;
using Gyminize.Models;
using Gyminize.Services;
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
    private ILocalSettingsService _localsettings;
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

    private bool _isSelectionNeeded;
    public bool IsSelectionNeeded
    {
        get => _isSelectionNeeded;
        set => SetProperty(ref _isSelectionNeeded, value);
    }


    // Khởi tạo ShellViewModel với INavigationService và INavigationViewService.
    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService, ILocalSettingsService localSettingsService)
    {
        _localsettings = localSettingsService;
        NavigationService = navigationService;
        CheckPlanSelectionNeeded();
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
        //IsSelectionNeeded = false;
    }

    public async void CheckPlanSelectionNeeded()
    {
        var customer_id = await _localsettings.ReadSettingAsync<string>("customer_id");
        var endpoint = "";
        endpoint = $"api/Plandetail/get/plandetail/" + customer_id;
        Plandetail plandetail = ApiServices.Get<Plandetail>(endpoint);
        // Kiểm tra kết quả
        if (plandetail != null)
        {
            IsSelectionNeeded = false;
        }
        else
        {
            IsSelectionNeeded = true;
        }
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