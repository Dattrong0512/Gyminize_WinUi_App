using System.Diagnostics.CodeAnalysis;

using CommunityToolkit.WinUI.UI.Animations;

using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;
using Gyminize.Helpers;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Gyminize.Services;

/// <summary>
/// Lớp này chịu trách nhiệm cung cấp dịch vụ điều hướng.
/// </summary>
public class NavigationService : INavigationService
{
    private readonly IPageService _pageService;
    private object? _lastParameterUsed;
    private Frame? _frame;

    // Sự kiện được kích hoạt khi điều hướng hoàn tất.
    public event NavigatedEventHandler? Navigated;

    // Thuộc tính Frame để điều hướng.
    public Frame? Frame
    {
        get
        {
            if (_frame == null)
            {
                // Lấy Frame từ MainWindow nếu chưa có.
                _frame = App.MainWindow.Content as Frame;
                RegisterFrameEvents();
            }

            return _frame;
        }

        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    // Thuộc tính kiểm tra xem có thể quay lại trang trước hay không.
    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => Frame != null && Frame.CanGoBack;

    // Khởi tạo NavigationService với IPageService.
    public NavigationService(IPageService pageService)
    {
        _pageService = pageService;
    }

    // Đăng ký sự kiện Navigated cho Frame.
    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated += OnNavigated;
        }
    }

    // Hủy đăng ký sự kiện Navigated cho Frame.
    private void UnregisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated -= OnNavigated;
        }
    }

    // Quay lại trang trước.
    public bool GoBack()
    {
        if (CanGoBack)
        {
            // Lấy ViewModel của trang trước khi điều hướng.
            var vmBeforeNavigation = _frame.GetPageViewModel();
            _frame.GoBack();
            if (vmBeforeNavigation is INavigationAware navigationAware)
            {
                // Gọi OnNavigatedFrom trên ViewModel của trang trước khi điều hướng.
                navigationAware.OnNavigatedFrom();
            }

            return true;
        }

        return false;
    }

    // Điều hướng đến trang với khóa trang và tham số tùy chọn.
    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = _pageService.GetPageType(pageKey);

        if (_frame != null && (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed))))
        {
            _frame.Tag = clearNavigation;
            var vmBeforeNavigation = _frame.GetPageViewModel();
            var navigated = _frame.Navigate(pageType, parameter);
            if (navigated)
            {
                _lastParameterUsed = parameter;
                if (vmBeforeNavigation is INavigationAware navigationAware)
                {
                    // Gọi OnNavigatedFrom trên ViewModel của trang trước khi điều hướng.
                    navigationAware.OnNavigatedFrom();
                }
            }

            return navigated;
        }

        return false;
    }

    // Xử lý sự kiện Navigated của Frame.
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                // Xóa BackStack nếu clearNavigation là true.
                frame.BackStack.Clear();
            }

            if (frame.GetPageViewModel() is INavigationAware navigationAware)
            {
                // Gọi OnNavigatedTo trên ViewModel của trang sau khi điều hướng.
                navigationAware.OnNavigatedTo(e.Parameter);
            }

            // Kích hoạt sự kiện Navigated.
            Navigated?.Invoke(sender, e);
        }
    }

    // Thiết lập mục dữ liệu danh sách cho hoạt ảnh kết nối tiếp theo.
    public void SetListDataItemForNextConnectedAnimation(object item) => Frame.SetListDataItemForNextConnectedAnimation(item);
}
