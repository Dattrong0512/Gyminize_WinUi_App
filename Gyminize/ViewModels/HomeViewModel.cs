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
using System.Net.WebSockets;
using Microsoft.UI.Xaml.Media;

namespace Gyminize.ViewModels;

public partial class HomeViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IWindowService _windowService;
    private bool _isWeightTextBoxEnabled;
    public string _customer_id;
    private CustomerHealth _customerHealth;
    public bool IsWeightTextBoxEnabled
    {
        get => _isWeightTextBoxEnabled;
        private set => SetProperty(ref _isWeightTextBoxEnabled, value);
    }
    public ILocalSettingsService localsetting;

    

    private Customer _customer;
    [ObservableProperty]
    private string weightText;
    [ObservableProperty]
    private string goalCalories;

    private string _remainCalories;
    public string RemainCalories
    {
        get => _remainCalories;
        set => SetProperty(ref _remainCalories, value);
    }

    private string _burnedCalories;
    public string BurnedCalories
    {
        get => _burnedCalories;
        set => SetProperty(ref _burnedCalories, value);
    }

    private string username;
    private double _progressValue;
    public double ProgressValue
    {
        get => _progressValue;
        set => SetProperty(ref _progressValue, value);
    }

    private string _typeWorkoutDate;

    public string TypeWorkoutDate
    {   
        get => _typeWorkoutDate;
        set => SetProperty(ref _typeWorkoutDate, value);
    }

    private string _statusIconPath;
    public string StatusIconPath
    {
        get => _statusIconPath;
        set => SetProperty(ref _statusIconPath, value);
    }

    private string _typeWorkoutIconPath;
    public string TypeWorkoutIconPath
    {
        get => _typeWorkoutIconPath;
        set => SetProperty(ref _typeWorkoutIconPath, value);
    }

    private string _exerciseStatus;

    public string ExerciseStatus
    {
        get => _exerciseStatus;
        set => SetProperty(ref _exerciseStatus, value);
    }

    private Visibility _statusVisibility;

    public Visibility StatusVisibility
    {   
        get => _statusVisibility;
        set => SetProperty(ref _statusVisibility, value);
    }

    private SolidColorBrush _progressForeground;

    public SolidColorBrush ProgressForeground
    {
        get => _progressForeground;
        set => SetProperty(ref _progressForeground, value);
    }
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
    public ICommand SaveWeightCommand
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
        SaveWeightCommand = new RelayCommand(OpenSaveWeight);
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


    private async void OpenSaveWeight()
    {

        IsWeightTextBoxEnabled = false;
        int weight = int.Parse(WeightText);
        //Thực hiện update weight lên csdl
        var endpoint = $"api/Customerhealth/update/" + _customer_id + "/weight/" + weight;
        var result = ApiServices.Put<CustomerHealth>(endpoint, null);
        if (result != null)
        {
            Debug.WriteLine("PUT request successful!");
        }
        else
        {
            Debug.WriteLine("PUT request failed.");
        }
    }
    public async void OnNavigatedTo(object parameter)
    {
        _customer_id = await localsetting.ReadSettingAsync<string>("customer_id");
        var endpoint = $"api/Customerhealth/get/" + _customer_id;

        // Sử dụng hàm Get<T> từ ApiServices để lấy dữ liệu
        _customerHealth = ApiServices.Get<CustomerHealth>(endpoint);

        var day = DateTime.UtcNow;
        var CurrentDailydiary = ApiServices.Get<Dailydiary>($"api/Dailydiary/get/daily_customer/{_customer_id}/day/{day:yyyy-MM-dd HH:mm:ss}");
        if (CurrentDailydiary != null)
        {
            WeightText = CurrentDailydiary.daily_weight.ToString();
            GoalCalories = ((int)CurrentDailydiary.total_calories).ToString();
            ProgressValue = (((double)CurrentDailydiary.total_calories - (double)CurrentDailydiary.calories_remain) / (double)CurrentDailydiary.total_calories) * 100;
            RemainCalories = ((int)CurrentDailydiary.calories_remain).ToString();
            BurnedCalories = ((int)CurrentDailydiary.total_calories - (int)CurrentDailydiary.calories_remain).ToString(); 
            if(ProgressValue > 100)
            {
                ProgressForeground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
            }
        }
        else
        {
            Dailydiary newDailydiary = new Dailydiary();
            newDailydiary.customer_id = _customerHealth.customer_id;
            newDailydiary.diary_date = day;
            newDailydiary.daily_weight = _customerHealth.weight;
            newDailydiary.calories_remain = Convert.ToInt32(_customerHealth.tdee);
            newDailydiary.total_calories = Convert.ToInt32(_customerHealth.tdee);
            newDailydiary.notes = "nothing";
            var newDailyDiary = ApiServices.Post<Dailydiary>("api/Dailydiary/create", newDailydiary);
            
            WeightText = newDailydiary.daily_weight.ToString();
            GoalCalories = newDailydiary.total_calories.ToString();
            RemainCalories = newDailydiary.calories_remain.ToString();
            BurnedCalories = 0.ToString();
            ProgressValue = 0;
        }

        var plandetails = ApiServices.Get<Plandetail>($"api/Plandetail/get/plandetail/{_customer_id}");
        if (plandetails != null)
        {
            Plandetail currentPlandetail = plandetails;
            var WorkoutDetailsItems = currentPlandetail.Workoutdetails.ToList();
            var currentDayWorkoutDetail = WorkoutDetailsItems.FirstOrDefault(item => item.IsCurrentDay);
            if (currentDayWorkoutDetail != null)
            {
                TypeWorkoutIconPath = "ms-appx:///Assets/Icon/arm.svg";
                TypeWorkoutDate = currentDayWorkoutDetail.Typeworkout.description;
                StatusVisibility = Visibility.Visible;
                if (currentDayWorkoutDetail.description == "Đã hoàn thành Exercise trong ngày") 
                {
                    ExerciseStatus = "Đã hoàn thành";
                    StatusIconPath = "ms-appx:///Assets/Icon/ok.svg";
                } 
                else
                {
                    ExerciseStatus = "Chưa hoàn thành";
                    StatusIconPath = "ms-appx:///Assets/Icon/error.svg";
                }
            } 
            else
            {
                TypeWorkoutIconPath = "ms-appx:///Assets/Icon/bed.svg";
                TypeWorkoutDate = "Ngày nghỉ";
                StatusVisibility = Visibility.Collapsed;
            }
        } 
        else
        {
            TypeWorkoutIconPath = "ms-appx:///Assets/Icon/gymplan.svg";
            TypeWorkoutDate = "Chưa có kế hoạch";
            StatusVisibility = Visibility.Collapsed;
        }

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
