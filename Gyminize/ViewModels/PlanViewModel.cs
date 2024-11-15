// ViewModel cho trang kế hoạch.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Gyminize.Core.Services;
using Gyminize.Helpers;
using Gyminize.Models;
namespace Gyminize.ViewModels;

public partial class PlanViewModel : ObservableRecipient
{
    // Khởi tạo PlanViewModel.
    private readonly INavigationService _navigationService;
    private string _planName;
    public string PlanName
    {
        get => _planName;
        set => SetProperty(ref _planName, value);
    }

    private int _weekNumber;
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


    public ICommand SelectWorkoutDetailCommand
    {
        get;
    }
    public ObservableCollection<Exercisedetail> ExerciseItems { get; set; } = new ObservableCollection<Exercisedetail>();

    public ObservableCollection<Workoutdetail> WeekDaysItems { get; set; } = new ObservableCollection<Workoutdetail>();

    public List<Workoutdetail> WorkoutDetailsItems { get; set; } = new List<Workoutdetail>();
    public PlanViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        SelectWorkoutDetailCommand = new RelayCommand<Workoutdetail>(SelectWorkoutDetail);
        LoadSampleData();
        LoadCurrentWeekDays(_startDate);
        PlanName = "Ke Hoach 4 Ngay";
        TotalDay = (_endDate - _startDate).Days;
        DayProgress = ((double)CompleteDay / (double)TotalDay) * 100;

        //Lấy thông tin cho ngày hôm nay
        var currentDayWorkoutDetail = WeekDaysItems.FirstOrDefault(item => item.IsCurrentDay);
        if (currentDayWorkoutDetail != null)
        {
            SelectWorkoutDetail(currentDayWorkoutDetail);
        }
    }

    public static List<Workoutdetail> GetWeekWorkoutDetails(DateTime startDate, int weekNumber, List<Workoutdetail> workoutDetails)
    {
        // Tính toán ngày bắt đầu của tuần (mỗi tuần cách nhau 7 ngày)
        var weekStartDate = startDate.AddDays((weekNumber - 1) * 7);

        // Lấy 7 ngày của tuần đó
        var weekWorkoutDetails = new List<Workoutdetail>();
        for (int i = 0; i < 7; i++)
        {
            var currentDate = weekStartDate.AddDays(i);
            var workoutDetail = workoutDetails.FirstOrDefault(wd => wd.date_workout.Date == currentDate.Date);

            if (workoutDetail == null)
            {
                // Nếu không có thông tin trong danh sách input, tạo ngày lấp vào với TypeworkoutId = 0
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
    private void LoadSampleData()
    {
        try
        {
            int customer_id = 1;
            var plandetails = ApiServices.Get<Plandetail>($"api/Plandetail/get/plandetail/{customer_id}");
            DateTime currentDate = DateTime.Now;
  
            Plandetail currentPlandetail = plandetails;
            _startDate = currentPlandetail.start_date;
            _endDate = currentPlandetail.end_date;
            WorkoutDetailsItems = currentPlandetail.Workoutdetails.ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading workout details library: {ex.Message}");
        }
    }
    public void LoadCurrentWeekDays(DateTime startDate)
    {
        var currentDate = DateTime.Now;
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

        int typeworkout = selectedWorkoutDetail.typeworkout_id;
        if (typeworkout == 0)
        {
            //do nothing
        }
        else
        {
            ExerciseItems = new ObservableCollection<Exercisedetail>(selectedWorkoutDetail.Typeworkout.Exercisedetails.ToList());
            OnPropertyChanged(nameof(ExerciseItems)); 
        }
    }
}

