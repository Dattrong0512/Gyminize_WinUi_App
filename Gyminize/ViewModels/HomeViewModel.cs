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
    private readonly IWindowService _windowService;
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
   
    public HomeViewModel(INavigationService navigationService, IWindowService windowService ,ILocalSettingsService localSettings)
    {
        _navigationService = navigationService;
        _windowService = windowService;
        OpenWorkoutLinkCommand = new RelayCommand(OpenWorkoutLink);
        OpenSleepLinkCommand = new RelayCommand(OpenSleepLink);
        OpenRecipeLinkCommand = new RelayCommand(OpenRecipeLink);
        EditWeightCommand = new RelayCommand(OpenEditWeight);
        IsWeightTextBoxEnabled = false;
        _customer = new Customer();
        localsetting = localSettings;
        _windowService.SetWindowSize(1500, 800);

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

        //// API này dùng để tạo một plan mới cho customer, ở đây mặc định sẽ là plan 1, tức là 3 ngày 1 tuần
        //endpoint = "";
        //endpoint = $"api/Plandetail/create/customer_id/" + customer_id+"/plan/1";
        //var result = ApiServices.Post<Plandetail>(endpoint,null);
        //// Kiểm tra kết quả
        //if (result != null)
        //{
        //    Debug.WriteLine("POST request successful!");
        //}
        //else
        //{
        //    Debug.WriteLine("POST request failed.");
        //}
        //// API này dùng để kiểm tra xem nó trả về đúng cái đối tượng plandetail không, các đối tượng này đã được cấu hình ở model để link với nhau
        //endpoint = "";
        //endpoint = $"api/Plandetail/get/plandetail/" + customer_id;
        //Plandetail plandetail =  ApiServices.Get<Plandetail>(endpoint);
        //// Kiểm tra kết quả
        //if (plandetail != null)
        //{
        //    Debug.WriteLine("Get request successful!");
        //    Debug.WriteLine(plandetail);
        //    Debug.WriteLine(plandetail.Plan);
        //    Debug.WriteLine(plandetail.Workoutdetails);
        //    if (plandetail.Workoutdetails != null)
        //    {
        //        foreach (var workout in plandetail.Workoutdetails)
        //        {
        
        //            if (workout.Typeworkout?.Exercisedetails != null)
        //            {
        //                Debug.WriteLine(workout.Typeworkout);
        //                Debug.WriteLine(workout.Typeworkout.description);
        //                foreach (var exerciseDetail in workout.Typeworkout.Exercisedetails)
        //                {
        //                    Debug.WriteLine(exerciseDetail);
        //                    Debug.WriteLine(exerciseDetail.Exercise.exercise_name);
        //                }
        //            }
        //        }
        //    }

        //}
        //else
        //{
        //    Debug.WriteLine("POST request failed.");
        //}
    }

    public void OnNavigatedFrom()
    {

    }
}
