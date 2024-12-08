using Gyminize.Contracts.Services;
using Gyminize.Core.Contracts.Services;
using Gyminize.Core.Helpers;
using Gyminize.Helpers;
using Gyminize.Models;

using Microsoft.Extensions.Options;

using Windows.ApplicationModel;
using Windows.Storage;

namespace Gyminize.Services;

/// <summary>
/// Lớp này quản lý cài đặt cục bộ của ứng dụng, cho phép lưu trữ và đọc cài đặt từ các tệp cục bộ hoặc LocalSettings khi ứng dụng chạy dưới MSIX.
/// </summary>
public class LocalSettingsService : ILocalSettingsService
{
    private const string _defaultApplicationDataFolder = "Gyminize/ApplicationData"; ///< Tên thư mục mặc định cho dữ liệu ứng dụng.
    private const string _defaultLocalSettingsFile = "LocalSettings.json"; ///< Tên tệp cài đặt mặc định.

    private readonly IFileService _fileService; ///< Dịch vụ xử lý tệp.
    private readonly LocalSettingsOptions _options; ///< Các tùy chọn cấu hình cho cài đặt cục bộ.

    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); ///< Đường dẫn thư mục LocalApplicationData.
    private readonly string _applicationDataFolder; ///< Đường dẫn thư mục dữ liệu ứng dụng.
    private readonly string _localsettingsFile; ///< Đường dẫn tệp cài đặt cục bộ.

    private IDictionary<string, object> _settings; ///< Từ điển chứa các cài đặt.

    private bool _isInitialized; ///< Cờ xác định xem dịch vụ đã được khởi tạo hay chưa.

    /// <summary>
    /// Khởi tạo dịch vụ cài đặt cục bộ.
    /// </summary>
    /// <param name="fileService">Dịch vụ tệp để đọc và lưu tệp cài đặt.</param>
    /// <param name="options">Cấu hình tùy chọn cài đặt cục bộ từ tệp cấu hình.</param>
    public LocalSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options)
    {
        _fileService = fileService;
        _options = options.Value;

        // Đặt đường dẫn thư mục dữ liệu ứng dụng và tệp cài đặt cục bộ.
        _applicationDataFolder = Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? _defaultApplicationDataFolder);
        _localsettingsFile = _options.LocalSettingsFile ?? _defaultLocalSettingsFile;

        _settings = new Dictionary<string, object>();
    }

    /// <summary>
    /// Khởi tạo dịch vụ cài đặt cục bộ, đọc cài đặt từ tệp nếu chưa được khởi tạo.
    /// </summary>
    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            // Đọc cài đặt từ tệp cài đặt cục bộ.
            _settings = await Task.Run(() => _fileService.Read<IDictionary<string, object>>(_applicationDataFolder, _localsettingsFile)) ?? new Dictionary<string, object>();

            _isInitialized = true;
        }
    }

    /// <summary>
    /// Đọc cài đặt từ LocalSettings hoặc tệp cài đặt cục bộ.
    /// </summary>
    /// <typeparam name="T">Loại dữ liệu cài đặt cần đọc.</typeparam>
    /// <param name="key">Tên khóa của cài đặt cần đọc.</param>
    /// <returns>Trả về cài đặt đã được giải mã từ JSON, hoặc null nếu không tìm thấy.</returns>
    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        if (RuntimeHelper.IsMSIX)
        {
            // Đọc cài đặt từ ApplicationData.Current.LocalSettings nếu ứng dụng đang chạy dưới MSIX.
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }
        }
        else
        {
            // Đọc cài đặt từ tệp cài đặt cục bộ.
            await InitializeAsync();

            if (_settings != null && _settings.TryGetValue(key, out var obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }
        }

        return default;
    }

    /// <summary>
    /// Lưu cài đặt vào LocalSettings hoặc tệp cài đặt cục bộ.
    /// </summary>
    /// <typeparam name="T">Loại dữ liệu cài đặt cần lưu.</typeparam>
    /// <param name="key">Tên khóa cài đặt cần lưu.</param>
    /// <param name="value">Giá trị cài đặt cần lưu.</param>
    /// <returns>Trả về một tác vụ bất đồng bộ.</returns>
    public async Task SaveSettingAsync<T>(string key, T value)
    {
        if (RuntimeHelper.IsMSIX)
        {
            // Lưu cài đặt vào ApplicationData.Current.LocalSettings nếu ứng dụng đang chạy dưới MSIX.
            ApplicationData.Current.LocalSettings.Values[key] = await Json.StringifyAsync(value);
        }
        else
        {
            // Lưu cài đặt vào tệp cài đặt cục bộ.
            await InitializeAsync();

            _settings[key] = await Json.StringifyAsync(value);

            await Task.Run(() => _fileService.Save(_applicationDataFolder, _localsettingsFile, _settings));
        }
    }
}
