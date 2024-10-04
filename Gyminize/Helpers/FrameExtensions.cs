using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Helpers;

// Lớp mở rộng cho Frame
// Cung cấp phương thức để lấy ViewModel của trang hiện tại trong Frame.
public static class FrameExtensions
{
    // Lấy ViewModel của trang hiện tại trong Frame
    // Trả về ViewModel nếu có, ngược lại trả về null
    public static object? GetPageViewModel(this Frame frame) => frame?.Content?.GetType().GetProperty("ViewModel")?.GetValue(frame.Content, null);
}