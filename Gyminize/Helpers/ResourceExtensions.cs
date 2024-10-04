using Microsoft.Windows.ApplicationModel.Resources;

namespace Gyminize.Helpers;

// Lớp mở rộng cho tài nguyên
// Cung cấp phương thức để lấy chuỗi đã được bản địa hóa từ ResourceLoader.
public static class ResourceExtensions
{
    // Đối tượng ResourceLoader để tải tài nguyên
    private static readonly ResourceLoader _resourceLoader = new();

    // Lấy chuỗi đã được bản địa hóa dựa trên khóa tài nguyên
    public static string GetLocalized(this string resourceKey) => _resourceLoader.GetString(resourceKey);
}
