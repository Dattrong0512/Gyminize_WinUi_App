namespace Gyminize.Contracts.Services;

// Giao diện cho dịch vụ cài đặt cục bộ
// Dịch vụ này chịu trách nhiệm đọc và lưu trữ các cài đặt cục bộ của ứng dụng.
public interface ILocalSettingsService
{
    // Đọc cài đặt với khóa cụ thể và trả về giá trị của nó
    Task<T?> ReadSettingAsync<T>(string key);

    // Lưu cài đặt với khóa và giá trị cụ thể
    Task SaveSettingAsync<T>(string key, T value);
}
