// ViewModel cho trang kế hoạch.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;
using Gyminize.Core.Services;
using Gyminize.Helpers;
using Gyminize.Models;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Security.ExchangeActiveSyncProvisioning;
namespace Gyminize.ViewModels;

public partial class PlanViewModel : ObservableRecipient
{
    // Khởi tạo PlanViewModel.
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly ILocalSettingsService _localsetting;
    private readonly IApiServicesClient _apiServicesClient;
    private readonly IDateTimeProvider _dateTimeProvider;
    private string _planName;
    public string PlanName
    {
        get => _planName;
        set => SetProperty(ref _planName, value);
    }

    private int _weekNumber;
    
    private int _customerId;
    public int CustomerId
    {
        get => _customerId;
        set => SetProperty(ref _customerId, value);
    }

    public int WeekNumber
    {
        get => _weekNumber;
        set => SetProperty(ref _weekNumber, value);
    }

    private int _completeDay;
    public int CompleteDay
    {
        get => _completeDay;
        set => SetProperty(ref _completeDay, value);
    }

    private int _totalDay;
    public int TotalDay
    {
        get => _totalDay;
        set => SetProperty(ref _totalDay, value);
    }

    private double _dayProgress;
    public double DayProgress
    {
        get => _dayProgress;
        set => SetProperty(ref _dayProgress, value);
    }

    private String _day1;
    public String Day1
    {
        get => _day1;
        set => SetProperty(ref _day1, value);
    }

    private string _day2;
    public string Day2
    {
        get => _day2;
        set => SetProperty(ref _day2, value);
    }

    private string _day3;
    public string Day3
    {
        get => _day3;
        set => SetProperty(ref _day3, value);
    }

    private string _day4;
    public string Day4
    {
        get => _day4;
        set => SetProperty(ref _day4, value);
    }

    private string _day5;
    public string Day5
    {
        get => _day5;
        set => SetProperty(ref _day5, value);
    }

    private string _day6;
    public string Day6
    {
        get => _day6;
        set => SetProperty(ref _day6, value);
    }

    private string _day7;
    public string Day7
    {
        get => _day7;
        set => SetProperty(ref _day7, value);
    }

    private bool _isCurrentDay;
    public bool IsCurrentDay
    {
        get => _isCurrentDay;
        set => SetProperty(ref _isCurrentDay, value);
    }

    //private bool _isSelected;
    //public bool IsSelected
    //{
    //    get => _isSelected;
    //    set => SetProperty(ref _isSelected, value);
    //}
    private DateTime _startDate;
    public DateTime StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value);
    }

    private DateTime _endDate;
    public DateTime EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value);
    }

    private bool _isBreakDay;
    public bool IsBreakDay
    {
        get => _isBreakDay;
        set => SetProperty(ref _isBreakDay, value);
    }

    private bool _isWorkoutDay;
    public bool IsWorkoutDay
    {
        get => _isWorkoutDay;
        set => SetProperty(ref _isWorkoutDay, value);
    }

    private string _statusText;
    public string StatusText
    {
        get => _statusText;
        set => SetProperty(ref _statusText, value);
    }

    private string _startExerciseText;
    public string StartExerciseText
    {
        get => _startExerciseText;
        set => SetProperty(ref _startExerciseText, value);
    }

    private string _workoutDetailDescription;
    public string WorkoutDetailDescription
    {
        get => _workoutDetailDescription;
        set => SetProperty(ref _workoutDetailDescription, value);
    }
    private bool _isFinished;
    public bool IsFinished
    {
        get => _isFinished;
        set => SetProperty(ref _isFinished, value);
    }

    private Visibility _workoutButtonVisibility;
    public Visibility WorkoutButtonVisibility
    {
        get => _workoutButtonVisibility;
        set => SetProperty(ref _workoutButtonVisibility, value);
    }
    public ICommand SelectWorkoutDetailCommand
    {
        get; set;
    }

    public ICommand PlayingWorkoutExercisesCommand
    {
        get; set;
    }
    public ICommand ShowSingleExerciseVideoCommand
    {
        get; set;
    }
    public ObservableCollection<Exercisedetail> ExerciseItems { get; set; } = new ObservableCollection<Exercisedetail>();

    private ObservableCollection<Workoutdetail> _weekDaysItems = new ObservableCollection<Workoutdetail>();
    public ObservableCollection<Workoutdetail> WeekDaysItems
    {
        get => _weekDaysItems;
        set
        {
            SetProperty(ref _weekDaysItems, value);
            (SelectWorkoutDetailCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
    }

    public List<Workoutdetail> WorkoutDetailsItems { get; set; } = new List<Workoutdetail>();
    public PlanViewModel(INavigationService navigationService, IDialogService dialogService, ILocalSettingsService localSettings, IApiServicesClient apiServicesClient, IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _localsetting = localSettings;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _apiServicesClient = apiServicesClient;
        SelectWorkoutDetailCommand = new RelayCommand<Workoutdetail>(SelectWorkoutDetail);
        ShowSingleExerciseVideoCommand = new RelayCommand<Exercisedetail>(ShowSingleExerciseVideo);
        PlayingWorkoutExercisesCommand = new AsyncRelayCommand(PlayingWorkoutExercises);
        InitializeViewModelAsync();
    }

    private async void InitializeViewModelAsync()
    {
        await GetCustomerID();
        LoadPlanDetailData();
        LoadCurrentWeekDays(_startDate);

        TotalDay = (_endDate - _startDate).Days;
        DayProgress = ((double)CompleteDay / (double)TotalDay) * 100;

        var currentDayWorkoutDetail = WeekDaysItems.FirstOrDefault(item => item.IsCurrentDay);
        if (currentDayWorkoutDetail != null)
        {
            SelectWorkoutDetail(currentDayWorkoutDetail);
        }
    }
    
    public async Task GetCustomerID()
    {
        var customer_id = await _localsetting.ReadSettingAsync<string>("customer_id");
        CustomerId = int.Parse(customer_id);
    }

    public static List<Workoutdetail> GetWeekWorkoutDetails(DateTime startDate, int weekNumber, List<Workoutdetail> workoutDetails)
    {
        var weekStartDate = startDate.AddDays((weekNumber - 1) * 7);
        var weekWorkoutDetails = new List<Workoutdetail>();
        for (int i = 0; i < 7; i++)
        {
            var currentDate = weekStartDate.AddDays(i);
            var workoutDetail = workoutDetails.FirstOrDefault(wd => wd.date_workout.Date == currentDate.Date);
            if (workoutDetail == null)
            {
                workoutDetail = new Workoutdetail
                {
                    typeworkout_id = 0,
                    date_workout = currentDate
                };
            }
            weekWorkoutDetails.Add(workoutDetail);
        }
        return weekWorkoutDetails;
    }
    public void LoadPlanDetailData()
    {
        try
        {
            var plandetails = _apiServicesClient.Get<Plandetail>($"api/Plandetail/get/plandetail/{CustomerId}");
            Plandetail currentPlandetail = plandetails;
            _startDate = currentPlandetail.start_date;
            _endDate = currentPlandetail.end_date;
            PlanName = currentPlandetail.Plan.plan_name;
            WorkoutDetailsItems = currentPlandetail.Workoutdetails.ToList();
        }
        catch (Exception ex)
        {
            _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: " + ex.Message);
            Debug.WriteLine($"Error loading plan details: {ex.Message}");
        }
    }
    public void LoadCurrentWeekDays(DateTime startDate)
    {
        var currentDate = _dateTimeProvider.Now;
        var daysDifference = (currentDate - startDate).Days;
        var currentWeekNumber = (daysDifference / 7) + 1;
        CompleteDay = daysDifference;
        WeekNumber = currentWeekNumber;
        var weekWorkoutDetails = GetWeekWorkoutDetails(startDate, currentWeekNumber, WorkoutDetailsItems);
        WeekDaysItems = new ObservableCollection<Workoutdetail>(weekWorkoutDetails);
        Day1 = weekWorkoutDetails[0].date_workout.ToString("dd/MM");
        Day2 = weekWorkoutDetails[1].date_workout.ToString("dd/MM");
        Day3 = weekWorkoutDetails[2].date_workout.ToString("dd/MM");
        Day4 = weekWorkoutDetails[3].date_workout.ToString("dd/MM");
        Day5 = weekWorkoutDetails[4].date_workout.ToString("dd/MM");
        Day6 = weekWorkoutDetails[5].date_workout.ToString("dd/MM");
        Day7 = weekWorkoutDetails[6].date_workout.ToString("dd/MM");
        for (var i = 0; i < WeekDaysItems.Count(); i++)
        {
            if (WeekDaysItems[i].date_workout.Date == _dateTimeProvider.Now.Date && WeekDaysItems[i].typeworkout_id == 0)
            {
                StatusText = "Hôm nay là ngày nghỉ";
                WorkoutButtonVisibility = Visibility.Collapsed;
                break;
            }
            if (WeekDaysItems[i].date_workout.Date == _dateTimeProvider.Now.Date && WeekDaysItems[i].description == "Đã hoàn thành Exercise trong ngày")
            {
                StatusText = "Bạn đã hoàn thành bài tập ngày hôm nay (" + _dateTimeProvider.Now.ToString("dd/MM") + ")";
                StartExerciseText = "Tập lại";
            }
            else if(WeekDaysItems[i].date_workout.Date == _dateTimeProvider.Now.Date && WeekDaysItems[i].description != "Đã hoàn thành Exercise trong ngày")
            {
                StatusText = "Bạn vẫn chưa hoàn thành bài tập hôm nay (" + _dateTimeProvider.Now.ToString("dd/MM") + ")";
                StartExerciseText = "Bắt đầu bài tập";
            }
            WorkoutButtonVisibility = Visibility.Visible;
        }
    }
    public void SelectWorkoutDetail(Workoutdetail selectedWorkoutDetail)
    {
        for (var i = 0; i < WeekDaysItems.Count(); i++)
        {
            if (WeekDaysItems[i] == selectedWorkoutDetail)
            {
                WeekDaysItems[i].IsSelected = true;
            }
            else
            {
                WeekDaysItems[i].IsSelected = false;
            }
        }
        var typeworkout = selectedWorkoutDetail.typeworkout_id;
        if (typeworkout == 0)
        {
            IsBreakDay = true;
            IsWorkoutDay = !IsBreakDay;
            WorkoutDetailDescription = "Ngày nghỉ";
        }
        else
        {
            ExerciseItems = new ObservableCollection<Exercisedetail>(selectedWorkoutDetail.Typeworkout.Exercisedetails.ToList());
            OnPropertyChanged(nameof(ExerciseItems));
            IsBreakDay = false;
            IsWorkoutDay = !IsBreakDay;
            WorkoutDetailDescription = selectedWorkoutDetail.Typeworkout.description;
        }
    }

    public async Task PlayingWorkoutExercises()
    {
        var currentDayWorkoutDetail = WeekDaysItems.FirstOrDefault(item => item.date_workout.Date == _dateTimeProvider.Now.Date);
        if (currentDayWorkoutDetail?.Typeworkout?.Exercisedetails != null)
        {
            IsFinished = await _dialogService.ShowFullExerciseWorkoutDialogAsync(currentDayWorkoutDetail.Typeworkout.Exercisedetails.ToList());
        }
        if(IsFinished == true)
        {
            try
            {
                var endpoint = $"api/Workoutdetail/update/{currentDayWorkoutDetail.workoutdetail_id}/decription/Đã hoàn thành Exercise trong ngày";
                var result = _apiServicesClient.Put<Workoutdetail>(endpoint, null);
                StatusText = "Bạn đã hoàn thành bài tập ngày hôm nay (" + _dateTimeProvider.Now.ToString("dd/MM") + ")";
                StartExerciseText = "Tập lại";
            } catch
            {
                await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: không thể cập nhật trạng thái bài tập");
            }
        }
    }

    public void ShowSingleExerciseVideo(Exercisedetail ex)
    {
        var exercise = ex.Exercise;
        _dialogService.ShowExerciseVideoDialogAsync(exercise);
    }
}

