using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Contracts.Services;

// Giao diện cho dịch vụ NavigationView
// Dịch vụ này chịu trách nhiệm quản lý các mục và sự kiện của NavigationView.
public interface INavigationViewService
{
    // Thuộc tính danh sách các mục menu
    IList<object>? MenuItems { get; }

    // Thuộc tính mục cài đặt
    object? SettingsItem { get; }

    // Khởi tạo NavigationView với NavigationView cụ thể
    void Initialize(NavigationView navigationView);

    // Hủy đăng ký các sự kiện
    void UnregisterEvents();

    // Lấy mục được chọn dựa trên loại trang
    NavigationViewItem? GetSelectedItem(Type pageType);
}
