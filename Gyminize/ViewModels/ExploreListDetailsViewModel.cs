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

public partial class ExploreListDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private readonly ISampleDataService _influencerDataService;
    private readonly INavigationService _navigationService;
    private readonly ILocalSettingsService _localSettingsService;

    [ObservableProperty]
    private Influencer? selected;

    public ObservableCollection<Influencer> SampleItems { get; private set; } = new ObservableCollection<Influencer>();

    private SolidColorBrush? _vietNamAppBarButtonBackground;

    public SolidColorBrush VietNamAppBarButtonBackground
    {
        get => _vietNamAppBarButtonBackground ??= new SolidColorBrush(Microsoft.UI.Colors.LightBlue);
        set => SetProperty(ref _vietNamAppBarButtonBackground, value);
    }

    private SolidColorBrush? _worldAppBarButtonBackground;

    public SolidColorBrush WorldAppBarButtonBackground
    {
        get => _worldAppBarButtonBackground ??= new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        set => SetProperty(ref _worldAppBarButtonBackground, value);
    }

    public ICommand VietNamSelectedCommand
    {
        get;
    }
    public ICommand WorldSelectedCommand
    {
        get;
    }

    public ICommand YtbLinkButtonClickedCommand
    {
        get;
    }

    public ExploreListDetailsViewModel(ISampleDataService sampleDataService, INavigationService navigationService, ILocalSettingsService localSettingsService)
    {
        _sampleDataService = sampleDataService;
        _navigationService = navigationService;
        _localSettingsService = localSettingsService;
        VietNamSelectedCommand = new RelayCommand(OnVietNamSelected);
        WorldSelectedCommand = new RelayCommand(OnWorldSelected);
        YtbLinkButtonClickedCommand = new RelayCommand<string>(OnYtbLinkClicked);
    }



    public async void OnNavigatedTo(object parameter)
    {
        SampleItems.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetInfluencerListDetailsDataAsync();
        var filteredData = data.Where(p => p.influencer_country == "Việt Nam").ToList();
        foreach (var item in filteredData)
        {
            SampleItems.Add(item);
        }
        EnsureItemSelected();
    }

    private async void OnVietNamSelected()
    {
        SampleItems.Clear();

        // TODO: Replace with real data.
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

    private async void OnWorldSelected()
    {
        SampleItems.Clear();

        // TODO: Replace with real data.
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

    private void OnYtbLinkClicked(string link)
    {
        _localSettingsService.SaveSettingAsync("YtbLink", link);
        _navigationService.NavigateTo(typeof(WebViewViewModel).FullName!, link);
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        Selected ??= SampleItems.First();
    }
}
