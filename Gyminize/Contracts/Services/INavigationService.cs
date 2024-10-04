using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Gyminize.Contracts.Services;

// Giao diện cho dịch vụ điều hướng
// Dịch vụ này chịu trách nhiệm điều hướng giữa các trang trong ứng dụng.
public interface INavigationService
{
    // Sự kiện được kích hoạt khi điều hướng
    event NavigatedEventHandler Navigated;

    // Thuộc tính kiểm tra xem có thể quay lại trang trước hay không
    bool CanGoBack { get; }

    // Thuộc tính Frame để điều hướng
    Frame? Frame { get; set; }

    // Điều hướng đến trang với khóa trang và tham số tùy chọn
    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);

    // Quay lại trang trước
    bool GoBack();

    // Thiết lập mục dữ liệu danh sách cho hoạt ảnh kết nối tiếp theo
    void SetListDataItemForNextConnectedAnimation(object item);
}
