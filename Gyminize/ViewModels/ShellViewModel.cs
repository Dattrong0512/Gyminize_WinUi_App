// ViewModel cho Shell (khung chính của ứng dụng).
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Gyminize.Core.Services;
using Gyminize.Models;
using Gyminize.Services;
using Gyminize.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Gyminize.ViewModels;

/// \brief ViewModel cho Shell (khung chính của ứng dụng).
/// \details Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
public partial class ShellViewModel : ObservableRecipient
{
    // Thuộc tính kiểm tra xem có thể quay lại trang trước hay không.

    /// \brief Thuộc tính kiểm tra xem có thể quay lại trang trước hay không.
    [ObservableProperty]
    private bool isBackEnabled;

    /// \brief Thuộc tính lưu trữ mục được chọn trong NavigationView.
    [ObservableProperty]
    private object? selected;

    /// \brief Thuộc tính kiểm tra hiển thị page plan hay planselection.
    private bool _isSelectionNeeded;

    public bool IsSelectionNeeded
    {
        get => _isSelectionNeeded;
        set => SetProperty(ref _isSelectionNeeded, value);
    }

    /// \brief Dịch vụ điều hướng.
    public INavigationService NavigationService { get; }

    /// \brief Dịch vụ NavigationView.
    public INavigationViewService NavigationViewService { get; }

    private readonly ILocalSettingsService _localsettings;
    private Frame _frame;

    /// \brief Hàm khởi tạo ShellViewModel.
    /// \param navigationService Dịch vụ điều hướng.
    /// \param navigationViewService Dịch vụ điều hướng NavigationView.
    /// \param localSettingsService Dịch vụ lưu trữ cài đặt cục bộ.
    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService, ILocalSettingsService localSettingsService)
    {
        _localsettings = localSettingsService;
        NavigationService = navigationService;
        CheckPlanSelectionNeeded();
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    /// \brief Kiểm tra xem người dùng có cần chọn kế hoạch hay không.
    /// \details Dựa trên thông tin trong cài đặt và API, xác định trạng thái `IsSelectionNeeded`.
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

    /// \brief Gán đối tượng Frame từ ShellPage vào ViewModel.
    /// \param frame Đối tượng Frame của ứng dụng.
    public void SetFrame(Frame frame)
    {
        _frame = frame;
    }

    /// \brief Xử lý sự kiện điều hướng.
    /// \param sender Đối tượng phát sinh sự kiện.
    /// \param e Thông tin sự kiện điều hướng.
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