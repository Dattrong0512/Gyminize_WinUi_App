// Tệp MainWindow.xaml.cs chứa logic cho cửa sổ chính của ứng dụng.
using Gyminize.Helpers;

using Windows.UI.ViewManagement;

namespace Gyminize;

// Lớp MainWindow kế thừa từ WindowEx để đại diện cho cửa sổ chính của ứng dụng.
public sealed partial class MainWindow : WindowEx
{
    // DispatcherQueue để quản lý hàng đợi các tác vụ cần thực hiện trên giao diện người dùng.
    private Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;

    // UISettings để quản lý các cài đặt giao diện người dùng.
    private UISettings settings;

    // Constructor của lớp MainWindow.
    public MainWindow()
    {
        InitializeComponent();

        // Đặt biểu tượng cho cửa sổ ứng dụng.
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        // Đặt nội dung ban đầu của cửa sổ là null.
        Content = null;
        // Đặt tiêu đề cho cửa sổ ứng dụng.
        Title = "AppDisplayName".GetLocalized();

        // Mã thay đổi chủ đề được lấy từ https://github.com/microsoft/WinUI-Gallery/pull/1239
        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        settings = new UISettings();
        // Đăng ký sự kiện thay đổi màu sắc của hệ thống.
        settings.ColorValuesChanged += Settings_ColorValuesChanged; // không thể sử dụng sự kiện FrameworkElement.ActualThemeChanged
    }

    // Phương thức xử lý sự kiện thay đổi màu sắc của hệ thống.
    // Phương thức này cập nhật màu sắc của các nút tiêu đề khi chủ đề hệ thống thay đổi trong khi ứng dụng đang mở.
    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // Lời gọi này được thực hiện ngoài luồng, do đó cần phải đưa nó vào hàng đợi của luồng hiện tại của ứng dụng.
        dispatcherQueue.TryEnqueue(() =>
        {
            // Áp dụng chủ đề hệ thống cho các nút tiêu đề.
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }
}

