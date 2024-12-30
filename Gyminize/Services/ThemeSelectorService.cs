using Gyminize.Contracts.Services;
using Gyminize.Helpers;

using Microsoft.UI.Xaml;

namespace Gyminize.Services;

/// <summary>
/// Lớp này chịu trách nhiệm chọn chủ đề cho ứng dụng.
/// </summary>
public class ThemeSelectorService : IThemeSelectorService
{
    private const string SettingsKey = "AppBackgroundRequestedTheme";

    // Thuộc tính Theme để lưu trữ chủ đề hiện tại.
    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    private readonly ILocalSettingsService _localSettingsService;

    // Khởi tạo ThemeSelectorService với ILocalSettingsService.
    public ThemeSelectorService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    // Khởi tạo dịch vụ và tải chủ đề từ cài đặt.
    public async Task InitializeAsync()
    {
        Theme = await LoadThemeFromSettingsAsync();
        await Task.CompletedTask;
    }

    // Đặt chủ đề và lưu vào cài đặt.
    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(Theme);
    }

    // Áp dụng chủ đề đã chọn cho ứng dụng.
    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(Theme);
        }

        await Task.CompletedTask;
    }

    // Tải chủ đề từ cài đặt.
    private async Task<ElementTheme> LoadThemeFromSettingsAsync()
    {
        var themeName = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (Enum.TryParse(themeName, out ElementTheme cacheTheme))
        {
            return cacheTheme;
        }

        return ElementTheme.Default;
    }

    // Lưu chủ đề vào cài đặt.
    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
    }
}
