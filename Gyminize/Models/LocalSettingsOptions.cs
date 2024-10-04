// Namespace chứa các mô hình của ứng dụng
namespace Gyminize.Models;

// Lớp đại diện cho các tùy chọn cài đặt cục bộ
public class LocalSettingsOptions
{
    // Thuộc tính để lấy hoặc đặt thư mục dữ liệu ứng dụng
    public string? ApplicationDataFolder
    {
        get; set;
    }

    // Thuộc tính để lấy hoặc đặt tệp cài đặt cục bộ
    public string? LocalSettingsFile
    {
        get; set;
    }
}