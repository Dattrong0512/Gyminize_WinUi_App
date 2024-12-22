using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;
using Gyminize.Core.Services;
using Gyminize.Models;
using Gyminize.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Gyminize.ViewModels;

/// <summary>
/// ViewModel cho màn hình chọn kế hoạch (Plan Selection).
/// </summary>
/// <remarks>
/// Lớp này chịu trách nhiệm quản lý các lệnh và dữ liệu cho việc chọn kế hoạch của người dùng. 
/// Nó cung cấp các lệnh để người dùng lựa chọn các kế hoạch khác nhau (Plan1, Plan2, Plan3), 
/// và thực hiện các thao tác như gửi yêu cầu API để tạo các chi tiết kế hoạch cho người dùng.
/// </remarks>
public partial class PlanSelectionViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly ILocalSettingsService _localSettingsService;
    private UIElement? _shell = null;

    /// <summary>
    /// Lệnh được gọi khi người dùng chọn kế hoạch 1.
    /// </summary>
    public ICommand Plan1SelectedCommand
    {
        get; set;
    }

    /// <summary>
    /// Lệnh được gọi khi người dùng chọn kế hoạch 2.
    /// </summary>
    public ICommand Plan2SelectedCommand
    {
        get; set;
    }

    /// <summary>
    /// Lệnh được gọi khi người dùng chọn kế hoạch 3.
    /// </summary>
    public ICommand Plan3SelectedCommand
    {
        get; set;
    }

    /// <summary>
    /// ID của khách hàng (customer_id).
    /// </summary>
    public string customer_id;

    /// <summary>
    /// Khởi tạo ViewModel cho việc chọn kế hoạch.
    /// </summary>
    /// <param name="navigationService">Dịch vụ điều hướng để chuyển trang.</param>
    /// <param name="localSettingsService">Dịch vụ lưu trữ cài đặt người dùng.</param>
    public PlanSelectionViewModel(INavigationService navigationService, ILocalSettingsService localSettingsService, IDialogService dialogService)
    {
        _navigationService = navigationService;
        _localSettingsService = localSettingsService;
        _dialogService = dialogService;
        Plan1SelectedCommand = new RelayCommand(Plan1Selection);
        Plan2SelectedCommand = new RelayCommand(Plan2Selection);
        Plan3SelectedCommand = new RelayCommand(Plan3Selection);
    }

    /// <summary>
    /// Thực thi lệnh khi người dùng chọn kế hoạch 1.
    /// </summary>
    public async void Plan1Selection()
    {
        var endpoint = $"api/Plandetail/create/customer_id/{customer_id}/plan/1";
        var result = ApiServices.Post<Plandetail>(endpoint, null);
        var frame = new Frame();
        _shell = App.GetService<ShellPage>();
        frame.Content = _shell;
        App.MainWindow.Content = frame;
        await _dialogService.ShowMarkdownDialogFromFileAsync("Assets/alert.txt");
        _navigationService.NavigateTo(typeof(PlanViewModel).FullName);
    }

    /// <summary>
    /// Thực thi lệnh khi người dùng chọn kế hoạch 2.
    /// </summary>
    public async void Plan2Selection()
    {
        var endpoint = $"api/Plandetail/create/customer_id/{customer_id}/plan/2";
        var result = ApiServices.Post<Plandetail>(endpoint, null);
        var frame = new Frame();
        _shell = App.GetService<ShellPage>();
        frame.Content = _shell;
        App.MainWindow.Content = frame;
        await _dialogService.ShowMarkdownDialogFromFileAsync("Assets/alert.txt");
        _navigationService.NavigateTo(typeof(PlanViewModel).FullName);
    }

    /// <summary>
    /// Thực thi lệnh khi người dùng chọn kế hoạch 3.
    /// </summary>
    public async void Plan3Selection()
    {
        var endpoint = $"api/Plandetail/create/customer_id/{customer_id}/plan/3";
        var result = ApiServices.Post<Plandetail>(endpoint, null);
        var frame = new Frame();
        _shell = App.GetService<ShellPage>();
        frame.Content = _shell;
        App.MainWindow.Content = frame;
        await _dialogService.ShowMarkdownDialogFromFileAsync("Assets/alert.txt");
        _navigationService.NavigateTo(typeof(PlanViewModel).FullName);
    }

    /// <summary>
    /// Được gọi khi ViewModel được điều hướng đến.
    /// Lấy customer_id từ cài đặt và lưu vào biến `customer_id`.
    /// </summary>
    /// <param name="parameter">Tham số được truyền từ màn hình trước.</param>
    public async void OnNavigatedTo(object parameter)
    {
        customer_id = await _localSettingsService.ReadSettingAsync<string>("customer_id");
    }

    /// <summary>
    /// Được gọi khi ViewModel được điều hướng đi.
    /// </summary>
    public void OnNavigatedFrom()
    {
        // Không có thao tác đặc biệt khi điều hướng đi
    }
}