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
using CommunityToolkit.Common;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.UI.Text;


namespace Gyminize.ViewModels;

public partial class HomeViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IWindowService _windowService;
    private readonly IDialogService _dialogService;
    private readonly IApiServicesClient _apiServicesClient;
    private readonly IDateTimeProvider _dateTimeProvider;

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
    private string weightTemp;
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

    private bool _isOverGoalCalories;

    public bool IsOverGoalCalories
    {
        get => _isOverGoalCalories;
        set => SetProperty(ref _isOverGoalCalories, value);
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

    public class WeightDataPoint
    {
        public DateTime Date
        {
            get; set;
        }
        public double Weight
        {
            get; set;
        }
    }

    private ObservableCollection<Line> _chartLines;
    private ObservableCollection<Line> _axisLines;
    private ObservableCollection<TextBlock> _axisLabels;

    public ObservableCollection<Line> ChartLines
    {
        get => _chartLines;
        set
        {
            _chartLines = value;
            OnPropertyChanged(nameof(ChartLines));
        }
    }

    public ObservableCollection<Line> AxisLines
    {
        get => _axisLines;
        set
        {
            _axisLines = value;
            OnPropertyChanged(nameof(AxisLines));
        }
    }

    public ObservableCollection<TextBlock> AxisLabels
    {
        get => _axisLabels;
        set
        {
            _axisLabels = value;
            OnPropertyChanged(nameof(AxisLabels));
        }
    }

    public List<WeightDataPoint> DataPoints
    {
        get; set;
    }

    public HomeViewModel(INavigationService navigationService, IWindowService windowService, ILocalSettingsService localSettings, IDialogService dialogService, IApiServicesClient apiServicesClient, IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _apiServicesClient = apiServicesClient;
        _dateTimeProvider = dateTimeProvider;
        _dialogService = dialogService;
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


        GenerateChartLines();
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


    public async void OpenSaveWeight()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(WeightText) || !WeightText.IsNumeric())
            {
                throw new ArgumentException("Cân nặng phải là một số");
            }

            int weight = int.Parse(WeightText);

            if (weight < 30 || weight > 200)
            {
                throw new ArgumentOutOfRangeException("Ứng dụng chỉ hỗ trợ cân nặng từ 30kg đến 200kg.");
            }
            var endpoint = $"api/Customerhealth/update/" + _customer_id + "/weight/" + weight;
            var result = _apiServicesClient.Put<CustomerHealth>(endpoint, null);
            weightTemp = weight.ToString();
            IsWeightTextBoxEnabled = false;
            await _dialogService.ShowSuccessMessageAsync("Cập nhật cân nặng thành công");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Debug.WriteLine($"Lỗi: {ex.Message}");
            await _dialogService.ShowErrorDialogAsync($"Lỗi: Ứng dụng chỉ hỗ trợ cân nặng từ 30kg đến 200kg");
            WeightText = weightTemp;
            IsWeightTextBoxEnabled = false;
        }
        catch (ArgumentException ex)
        {
            Debug.WriteLine($"Lỗi: {ex.Message}");
            await _dialogService.ShowErrorDialogAsync($"Lỗi: {ex.Message}");
            WeightText = weightTemp;
            IsWeightTextBoxEnabled = false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Lỗi hệ thống: {ex.Message}");
            await _dialogService.ShowErrorDialogAsync($"Lỗi hệ thống: {ex.Message}");
            WeightText = weightTemp;
            IsWeightTextBoxEnabled = false;
        }
    }
    public async void OnNavigatedTo(object parameter)
    {
        _customer_id = await localsetting.ReadSettingAsync<string>("customer_id");
        var endpoint = $"api/Customerhealth/get/" + _customer_id;

        _customerHealth = _apiServicesClient.Get<CustomerHealth>(endpoint);
        var day = _dateTimeProvider.UtcNow;
        var CurrentDailydiary = _apiServicesClient.Get<Dailydiary>($"api/Dailydiary/get/daily_customer/{_customer_id}/day/{day:yyyy-MM-dd HH:mm:ss}");
        if (CurrentDailydiary != null)
        {
            weightTemp = CurrentDailydiary.daily_weight.ToString();
            WeightText = CurrentDailydiary.daily_weight.ToString();
            GoalCalories = ((int)CurrentDailydiary.total_calories).ToString();
            ProgressValue = (((double)CurrentDailydiary.total_calories - (double)CurrentDailydiary.calories_remain) / (double)CurrentDailydiary.total_calories) * 100;
            RemainCalories = ((int)CurrentDailydiary.calories_remain).ToString();
            BurnedCalories = ((int)CurrentDailydiary.total_calories - (int)CurrentDailydiary.calories_remain).ToString();
            IsOverGoalCalories = ProgressValue > 100 ? true : false;
        }
        else
        {
            try
            {
                Dailydiary newDailydiary = new Dailydiary();
                newDailydiary.customer_id = _customerHealth.customer_id;
                newDailydiary.diary_date = day;
                newDailydiary.daily_weight = _customerHealth.weight;
                newDailydiary.calories_remain = Convert.ToInt32(_customerHealth.tdee);
                newDailydiary.total_calories = Convert.ToInt32(_customerHealth.tdee);
                newDailydiary.notes = "nothing";
                var newDailyDiary = _apiServicesClient.Post<Dailydiary>("api/Dailydiary/create", newDailydiary);

                WeightText = newDailydiary.daily_weight.ToString();
                GoalCalories = newDailydiary.total_calories.ToString();
                RemainCalories = newDailydiary.calories_remain.ToString();
                BurnedCalories = 0.ToString();
                ProgressValue = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("POST request failed.");
                await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống:" + ex.Message);
            }
        }

        var plandetails = _apiServicesClient.Get<Plandetail>($"api/Plandetail/get/plandetail/{_customer_id}");
        if (plandetails != null)
        {
            Plandetail currentPlandetail = plandetails;
            var WorkoutDetailsItems = currentPlandetail.Workoutdetails.ToList();
            var currentDayWorkoutDetail = WorkoutDetailsItems.FirstOrDefault(item => item.date_workout == _dateTimeProvider.Now);
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
        //var result = _apiServicesClient.Post<Plandetail>(endpoint,null);
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
        //Plandetail plandetail =  _apiServicesClient.Get<Plandetail>(endpoint);
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

    

    private async void GenerateChartLines()
    {
        ChartLines = new ObservableCollection<Line>();
        AxisLines = new ObservableCollection<Line>();
        AxisLabels = new ObservableCollection<TextBlock>();

        //// Example data points
        //DataPoints = new List<WeightDataPoint>
        //{
        //    new WeightDataPoint { Date = DateTime.Now.AddDays(-7), Weight = 70 },
        //    new WeightDataPoint { Date = DateTime.Now.AddDays(-6), Weight = 71 },
        //    new WeightDataPoint { Date = DateTime.Now.AddDays(-5), Weight = 69 },
        //    new WeightDataPoint { Date = DateTime.Now.AddDays(-4), Weight = 70 },
        //    new WeightDataPoint { Date = DateTime.Now.AddDays(-3), Weight = 66 },
        //    new WeightDataPoint { Date = DateTime.Now.AddDays(-2), Weight = 67 },
        //    new WeightDataPoint { Date = DateTime.Now.AddDays(-1), Weight = 70 },

        //};

        _customer_id = await localsetting.ReadSettingAsync<string>("customer_id");
        var endpoint = $"api/Customerhealth/get/" + _customer_id;

        _customerHealth = _apiServicesClient.Get<CustomerHealth>(endpoint);

        DataPoints = new List<WeightDataPoint>();
        DateTime today = _dateTimeProvider.UtcNow;
        double currentWeight = _customerHealth.weight; 
        double lastKnownWeight = currentWeight;

        for (int i = 7; i > 0; i--)
        {
            DateTime day = today.AddDays(-i);
            Dailydiary currentDailydiary = null;

            currentDailydiary = _apiServicesClient.Get<Dailydiary>($"api/Dailydiary/get/daily_customer/{_customer_id}/day/{day:yyyy-MM-dd HH:mm:ss}");

            if (currentDailydiary == null)
            {
                DataPoints.Add(new WeightDataPoint
                {
                    Date = day,
                    Weight = lastKnownWeight
                });
            }
            else
            {
                lastKnownWeight = currentDailydiary.daily_weight;
                DataPoints.Add(new WeightDataPoint
                {
                    Date = day,
                    Weight = currentDailydiary.daily_weight
                });
            }
        }

        double canvasWidth = 300;
        double canvasHeight = 240;
        double maxWeight = DataPoints.Max(dp => dp.Weight) + 20;
        double minWeight = DataPoints.Min(dp => dp.Weight) - 20;
        double weightRange = maxWeight - minWeight;
        double xInterval = canvasWidth / (DataPoints.Count - 1);
        double marginLeft = 40; 
        double marginTop = 20; 

        AxisLabels.Add(new TextBlock
        {
            Text = "Biểu đồ cân nặng trong 7 ngày qua",
            Foreground = new SolidColorBrush(Colors.Black),
            FontSize = 16,
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(marginLeft + canvasWidth / 2 - 120, 0, 0, 0)
        });

        for (int i = 0; i < DataPoints.Count - 1; i++)
        {
            double y1 = marginTop + canvasHeight - ((DataPoints[i].Weight - minWeight) / weightRange * canvasHeight);
            double y2 = marginTop + canvasHeight - ((DataPoints[i + 1].Weight - minWeight) / weightRange * canvasHeight);

            var line = new Line
            {
                X1 = marginLeft + i * xInterval,
                Y1 = y1,
                X2 = marginLeft + (i + 1) * xInterval,
                Y2 = y2,
                Stroke = new SolidColorBrush(Colors.Blue),
                StrokeThickness = 2
            };
            ChartLines.Add(line);
        }

        AxisLines.Add(new Line { X1 = marginLeft, Y1 = marginTop + canvasHeight, X2 = marginLeft + canvasWidth, Y2 = marginTop + canvasHeight, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 1 });

        AxisLines.Add(new Line { X1 = marginLeft, Y1 = marginTop, X2 = marginLeft, Y2 = marginTop + canvasHeight, Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 1 });

        for (int i = 0; i < DataPoints.Count; i++)
        {
            AxisLabels.Add(new TextBlock { Text = DataPoints[i].Date.ToString("dd"), Foreground = new SolidColorBrush(Colors.Black), Margin = new Thickness(marginLeft + i * xInterval - 15, marginTop + canvasHeight + 5, 0, 0) });
        }

        for (int i = 0; i < DataPoints.Count; i++)
        {
            double yPosition = marginTop + canvasHeight - ((DataPoints[i].Weight - minWeight) / weightRange * canvasHeight);
            AxisLabels.Add(new TextBlock { Text = DataPoints[i].Weight.ToString("F1"), Foreground = new SolidColorBrush(Colors.Black), Margin = new Thickness(marginLeft - 30, yPosition - 10, 0, 0) });
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void OnNavigatedFrom()
    {

    }
}
