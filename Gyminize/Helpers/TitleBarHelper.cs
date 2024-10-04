using System.Runtime.InteropServices;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

using Windows.UI;
using Windows.UI.ViewManagement;

namespace Gyminize.Helpers;

// Lớp trợ giúp để khắc phục các lỗi thanh tiêu đề tùy chỉnh.
// LƯU Ý: Tên khóa tài nguyên và giá trị màu sắc được sử dụng dưới đây có thể thay đổi. Không phụ thuộc vào chúng.
// https://github.com/microsoft/TemplateStudio/issues/4516
internal class TitleBarHelper
{
    // Hằng số cho các trạng thái kích hoạt cửa sổ
    private const int WAINACTIVE = 0x00;
    private const int WAACTIVE = 0x01;
    private const int WMACTIVATE = 0x0006;

    // Khai báo hàm GetActiveWindow từ user32.dll
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    // Khai báo hàm SendMessage từ user32.dll
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

    // Phương thức cập nhật thanh tiêu đề dựa trên chủ đề
    public static void UpdateTitleBar(ElementTheme theme)
    {
        // Kiểm tra xem nội dung có mở rộng vào thanh tiêu đề không
        if (App.MainWindow.ExtendsContentIntoTitleBar)
        {
            // Nếu chủ đề là mặc định, lấy màu nền từ UISettings
            if (theme == ElementTheme.Default)
            {
                var uiSettings = new UISettings();
                var background = uiSettings.GetColorValue(UIColorType.Background);

                theme = background == Colors.White ? ElementTheme.Light : ElementTheme.Dark;
            }

            // Nếu chủ đề vẫn là mặc định, lấy chủ đề từ ứng dụng
            if (theme == ElementTheme.Default)
            {
                theme = Application.Current.RequestedTheme == ApplicationTheme.Light ? ElementTheme.Light : ElementTheme.Dark;
            }

            // Cập nhật màu sắc của các nút trên thanh tiêu đề dựa trên chủ đề
            App.MainWindow.AppWindow.TitleBar.ButtonForegroundColor = theme switch
            {
                ElementTheme.Dark => Colors.White,
                ElementTheme.Light => Colors.Black,
                _ => Colors.Transparent
            };

            App.MainWindow.AppWindow.TitleBar.ButtonHoverForegroundColor = theme switch
            {
                ElementTheme.Dark => Colors.White,
                ElementTheme.Light => Colors.Black,
                _ => Colors.Transparent
            };

            App.MainWindow.AppWindow.TitleBar.ButtonHoverBackgroundColor = theme switch
            {
                ElementTheme.Dark => Color.FromArgb(0x33, 0xFF, 0xFF, 0xFF),
                ElementTheme.Light => Color.FromArgb(0x33, 0x00, 0x00, 0x00),
                _ => Colors.Transparent
            };

            App.MainWindow.AppWindow.TitleBar.ButtonPressedBackgroundColor = theme switch
            {
                ElementTheme.Dark => Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF),
                ElementTheme.Light => Color.FromArgb(0x66, 0x00, 0x00, 0x00),
                _ => Colors.Transparent
            };

            App.MainWindow.AppWindow.TitleBar.BackgroundColor = Colors.Transparent;

            // Lấy handle của cửa sổ hiện tại
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            if (hwnd == GetActiveWindow())
            {
                // Gửi thông điệp để cập nhật trạng thái kích hoạt của cửa sổ
                SendMessage(hwnd, WMACTIVATE, WAINACTIVE, IntPtr.Zero);
                SendMessage(hwnd, WMACTIVATE, WAACTIVE, IntPtr.Zero);
            }
            else
            {
                SendMessage(hwnd, WMACTIVATE, WAACTIVE, IntPtr.Zero);
                SendMessage(hwnd, WMACTIVATE, WAINACTIVE, IntPtr.Zero);
            }
        }
    }

    // Phương thức áp dụng chủ đề hệ thống cho các nút trên thanh tiêu đề
    public static void ApplySystemThemeToCaptionButtons()
    {
        var frame = App.AppTitlebar as FrameworkElement;
        if (frame != null)
        {
            UpdateTitleBar(frame.ActualTheme);
        }
    }
}
