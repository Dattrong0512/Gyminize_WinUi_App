// Sử dụng directive cho namespace Microsoft.UI.Xaml
using Microsoft.UI.Xaml;

namespace Gyminize.Contracts.Services;

// Giao diện cho dịch vụ chọn chủ đề
public interface IThemeSelectorService
{
    // Thuộc tính để lấy chủ đề hiện tại
    ElementTheme Theme
    {
        get;
    }

    // Phương thức khởi tạo dịch vụ chọn chủ đề
    Task InitializeAsync();

    // Phương thức đặt chủ đề một cách bất đồng bộ
    Task SetThemeAsync(ElementTheme theme);

    // Phương thức đặt chủ đề yêu cầu một cách bất đồng bộ
    Task SetRequestedThemeAsync();
}