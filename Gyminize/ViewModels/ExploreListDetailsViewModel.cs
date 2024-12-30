using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Gyminize.Contracts.ViewModels;
using Gyminize.Core.Contracts.Services;
using Gyminize.Core.Models;
using Gyminize.Services;
using Gyminize.Contracts.Services;
using Microsoft.UI.Xaml.Media;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Gyminize.ViewModels;

/// <summary>
/// ViewModel cho chi tiết danh sách khám phá, cung cấp các thuộc tính và phương thức để quản lý danh sách Influencer.
/// </summary>
public partial class ExploreListDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private readonly INavigationService _navigationService;
    private readonly ILocalSettingsService _localSettingsService;

    /// <summary>
    /// Thuộc tính được đánh dấu là Observable cho Influencer được chọn.
    /// </summary>
    [ObservableProperty]
    private Influencer? selected;

    /// <summary>
    /// Nguồn dữ liệu cho danh sách Influencer.
    /// </summary>
    public ObservableCollection<Influencer> SampleItems { get; private set; } = new ObservableCollection<Influencer>();

    private SolidColorBrush? _vietNamAppBarButtonBackground;

    /// <summary>
    /// Màu nền cho nút AppBar của Việt Nam.
    /// </summary>
    public SolidColorBrush VietNamAppBarButtonBackground
    {
        get => _vietNamAppBarButtonBackground ??= new SolidColorBrush(Microsoft.UI.Colors.LightBlue);
        set => SetProperty(ref _vietNamAppBarButtonBackground, value);
    }

    private SolidColorBrush? _worldAppBarButtonBackground;

    /// <summary>
    /// Màu nền cho nút AppBar của Thế giới.
    /// </summary>
    public SolidColorBrush WorldAppBarButtonBackground
    {
        get => _worldAppBarButtonBackground ??= new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        set => SetProperty(ref _worldAppBarButtonBackground, value);
    }

    /// <summary>
    /// Lệnh được thực thi khi chọn Influencer từ Việt Nam.
    /// </summary>
    public ICommand VietNamSelectedCommand
    {
        get;
    }

    /// <summary>
    /// Lệnh được thực thi khi chọn Influencer từ Thế giới.
    /// </summary>
    public ICommand WorldSelectedCommand
    {
        get;
    }

    /// <summary>
    /// Lệnh được thực thi khi nhấn vào nút liên kết YouTube.
    /// </summary>
    public ICommand YtbLinkButtonClickedCommand
    {
        get;
    }

    /// <summary>
    /// Khởi tạo một thể hiện mới của lớp <see cref="ExploreListDetailsViewModel"/>.
    /// </summary>
    /// <param name="sampleDataService">Dịch vụ dữ liệu mẫu.</param>
    /// <param name="navigationService">Dịch vụ điều hướng.</param>
    /// <param name="localSettingsService">Dịch vụ cài đặt cục bộ.</param>
    public ExploreListDetailsViewModel(ISampleDataService sampleDataService, INavigationService navigationService, ILocalSettingsService localSettingsService)
    {
        _sampleDataService = sampleDataService;
        _navigationService = navigationService;
        _localSettingsService = localSettingsService;
        VietNamSelectedCommand = new RelayCommand(OnVietNamSelected);
        WorldSelectedCommand = new RelayCommand(OnWorldSelected);
        YtbLinkButtonClickedCommand = new RelayCommand<string>(OnYtbLinkClicked);
    }

    /// <summary>
    /// Phương thức được gọi khi điều hướng đến trang.
    /// </summary>
    /// <param name="parameter">Dữ liệu được truyền khi điều hướng.</param>
    public async void OnNavigatedTo(object parameter)
    {
        SampleItems.Clear();

        // TODO: Thay thế bằng dữ liệu thực.
        var data = await _sampleDataService.GetInfluencerListDetailsDataAsync();
        var filteredData = data.Where(p => p.influencer_country == "Việt Nam").ToList();
        foreach (var item in filteredData)
        {
            SampleItems.Add(item);
        }
        EnsureItemSelected();
    }

    /// <summary>
    /// Phương thức được gọi khi chọn Influencer từ Việt Nam.
    /// </summary>
    private async void OnVietNamSelected()
    {
        SampleItems.Clear();

        // TODO: Thay thế bằng dữ liệu thực.
        var data = await _sampleDataService.GetInfluencerListDetailsDataAsync();
        var filteredData = data.Where(p => p.influencer_country == "Việt Nam").ToList();
        foreach (var item in filteredData)
        {
            SampleItems.Add(item);
        }
        EnsureItemSelected();
        VietNamAppBarButtonBackground = new SolidColorBrush(Microsoft.UI.Colors.LightBlue);
        WorldAppBarButtonBackground = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
    }

    /// <summary>
    /// Phương thức được gọi khi chọn Influencer từ Thế giới.
    /// </summary>
    private async void OnWorldSelected()
    {
        SampleItems.Clear();

        // TODO: Thay thế bằng dữ liệu thực.
        var data = await _sampleDataService.GetInfluencerListDetailsDataAsync();
        var filteredData = data.Where(p => p.influencer_country != "Việt Nam").ToList();
        foreach (var item in filteredData)
        {
            SampleItems.Add(item);
        }
        EnsureItemSelected();
        VietNamAppBarButtonBackground = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        WorldAppBarButtonBackground = new SolidColorBrush(Microsoft.UI.Colors.LightBlue);
    }

    /// <summary>
    /// Phương thức được gọi khi nhấn vào nút liên kết YouTube.
    /// </summary>
    /// <param name="link">Liên kết YouTube.</param>
    private void OnYtbLinkClicked(string link)
    {
        _localSettingsService.SaveSettingAsync("YtbLink", link);
        _navigationService.NavigateTo(typeof(WebViewViewModel).FullName!, link);
    }

    /// <summary>
    /// Phương thức được gọi khi điều hướng đi từ trang.
    /// </summary>
    public void OnNavigatedFrom()
    {
    }

    /// <summary>
    /// Đảm bảo rằng một Influencer được chọn.
    /// </summary>
    public void EnsureItemSelected()
    {
        Selected ??= SampleItems.First();
    }
}
