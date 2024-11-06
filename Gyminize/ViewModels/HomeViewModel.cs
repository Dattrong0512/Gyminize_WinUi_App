// ViewModel cho trang chủ.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Gyminize.Models;
using Gyminize.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Gyminize.Core.Services;
using Gyminize.Contracts.ViewModels;
using System.Diagnostics;
using System.Windows.Input;
using System.Net;
using System.Runtime.InteropServices;
using Windows.Storage;

namespace Gyminize.ViewModels;

public partial class HomeViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;
    private bool _isWeightTextBoxEnabled;
    private CustomerHealth _customerHealth;
    public bool IsWeightTextBoxEnabled
    {
        get => _isWeightTextBoxEnabled;
        private set => SetProperty(ref _isWeightTextBoxEnabled, value);
    }
    public ILocalSettingsService localsetting;
    private int _weight;
    private Customer _customer;
    [ObservableProperty]
    private string weightText;
    [ObservableProperty]
    private string goalCalories;
    private string username;

    public RelayCommand OpenWorkoutLinkCommand
    {
        get;
    }
    public RelayCommand OpenSleepLinkCommand
    {
        get;
    }
    public RelayCommand OpenRecipeLinkCommand
    {
        get;
    }
    public ICommand EditWeightCommand
    {
        get;
    }
   
    public HomeViewModel(INavigationService navigationService, ILocalSettingsService localSettings)
    {
        _navigationService = navigationService;
        OpenWorkoutLinkCommand = new RelayCommand(OpenWorkoutLink);
        OpenSleepLinkCommand = new RelayCommand(OpenSleepLink);
        OpenRecipeLinkCommand = new RelayCommand(OpenRecipeLink);
        EditWeightCommand = new RelayCommand(OpenEditWeight);
        IsWeightTextBoxEnabled = false;
        _customer = new Customer();
        localsetting = localSettings;


    }

    private void OpenWorkoutLink()
    {
        var uri = new Uri("https://darebee.com/workouts.html");
        Process.Start(new ProcessStartInfo
        {
            FileName = uri.AbsoluteUri,
            UseShellExecute = true
        });
    }

    private void OpenRecipeLink()
    {
        var uri = new Uri("https://www.allrecipes.com/recipes/");
        Process.Start(new ProcessStartInfo
        {
            FileName = uri.AbsoluteUri,
            UseShellExecute = true
        });
    }

    private void OpenSleepLink()
    {
        var uri = new Uri("https://sleepdoctor.com/");
        Process.Start(new ProcessStartInfo
        {
            FileName = uri.AbsoluteUri,
            UseShellExecute = true
        });
    }

    private void OpenEditWeight()
    {
        IsWeightTextBoxEnabled = true;
    }

    public async void OnNavigatedTo(object parameter)
    {
        var customer_id = await localsetting.ReadSettingAsync<string>("customer_id");
        var endpoint = $"api/Customerhealth/get/" + customer_id;

        // Sử dụng hàm Get<T> từ ApiServices để lấy dữ liệu
        _customerHealth = ApiServices.Get<CustomerHealth>(endpoint);
        WeightText = _customerHealth.weight.ToString();
        GoalCalories = ((int)_customerHealth.tdee).ToString();
    }

    public void OnNavigatedFrom()
    {

    }
}
