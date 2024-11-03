// ViewModel cho trang chủ.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Gyminize.Models;
using Gyminize.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Gyminize.Contracts.ViewModels;
using System.Diagnostics;
using System.Windows.Input;
using Gyminize.APIServices;
using System.Net;
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

    public HomeViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        OpenWorkoutLinkCommand = new RelayCommand(OpenWorkoutLink);
        OpenSleepLinkCommand = new RelayCommand(OpenSleepLink);
        OpenRecipeLinkCommand = new RelayCommand(OpenRecipeLink);
        EditWeightCommand = new RelayCommand(OpenEditWeight);
        IsWeightTextBoxEnabled = false;
       
        _customer = new Customer();



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

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is string customerName)
        {
            string endpoint = $"api/Customer/get/username/" + customerName;

            // Sử dụng hàm Get<T> từ ApiServices để lấy dữ liệu
            _customer = ApiServices.Get<Customer>(endpoint);
            endpoint = "";
             endpoint = $"api/Customerhealth/get/" + _customer.customer_id;


            // Sử dụng hàm Get<T> từ ApiServices để lấy dữ liệu
            _customerHealth = ApiServices.Get<CustomerHealth>(endpoint);
            Debug.WriteLine(_customerHealth.weight);
            WeightText = _customerHealth.weight.ToString();
            GoalCalories = ((int)_customerHealth.tdee).ToString();
        }
    }

    public void OnNavigatedFrom()
    {

    }
}
