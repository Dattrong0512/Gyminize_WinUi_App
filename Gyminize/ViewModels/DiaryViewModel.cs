// ViewModel cho trang nhật ký.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Gyminize.Core.Services;
using Gyminize.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinUIEx;

namespace Gyminize.ViewModels;

/// <summary>
/// ViewModel cho trang nhật, hỗ trợ tương tác với UI, quản lý các hành động và xem trạng thái luyện tập dinh dưỡng của các ngày trước đó.
/// </summary>
public partial class DiaryViewModel : ObservableRecipient
{
    // Khởi tạo DiaryViewModel.
    private readonly ILocalSettingsService _localSettingsService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IApiServicesClient _apiServicesClient;
    private readonly IDialogService _dialogService;
    private string _customer_id;
    private int _weightText;

    public class DayStatus
    {
        public DateTime Date
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }
    }
    public List<DayStatus> DayStatuses { get; private set; } = new List<DayStatus>();
    private string _selectedDayText;
    public string SelectedDayText
    {
        get => _selectedDayText;
        set => SetProperty(ref _selectedDayText, value);
    }
    public int WeightText
    {
        get => _weightText;
        set => SetProperty(ref _weightText, value);
    }

    private string _planNameText;
    public string PlanNameText
    {
        get => _planNameText;
        set => SetProperty(ref _planNameText, value);
    }

    private string _typeWorkoutText;
    public string TypeWorkoutText
    {
        get => _typeWorkoutText;
        set => SetProperty(ref _typeWorkoutText, value);
    }

    private int _burnedCalories;
    public int BurnedCalories
    {
        get => _burnedCalories;
        set => SetProperty(ref _burnedCalories, value);
    }

    private int _totalCalories;
    public int TotalCalories
    {
        get => _totalCalories;
        set => SetProperty(ref _totalCalories, value);
    }

    public int _exerciseStatus;

    private bool _isLunchEmpty;
    public bool IsLunchEmpty
    {
        get => _isLunchEmpty;
        set => SetProperty(ref _isLunchEmpty, value);
    }
    private bool _isBreakfastEmpty;
    public bool IsBreakfastEmpty
    {
        get => _isBreakfastEmpty;
        set => SetProperty(ref _isBreakfastEmpty, value);
    }
    private bool _isSnackEmpty;
    public bool IsSnackEmpty
    {
        get => _isSnackEmpty;
        set => SetProperty(ref _isSnackEmpty, value);
    }
    private bool _isDinnerEmpty;
    public bool IsDinnerEmpty
    {
        get => _isDinnerEmpty;
        set => SetProperty(ref _isDinnerEmpty, value);
    }
    public ICommand SelectedDatesChangedCommand
    { 
        get;
    }

    public ICommand DecorateDayItemCommand
    { 
        get;
    }
    public ObservableCollection<FoodDetail> BreakfastItems { get; set; } = new ObservableCollection<FoodDetail>();
    public ObservableCollection<FoodDetail> LunchItems { get; set; } = new ObservableCollection<FoodDetail>();
    public ObservableCollection<FoodDetail> DinnerItems { get; set; } = new ObservableCollection<FoodDetail>();
    public ObservableCollection<FoodDetail> SnackItems { get; set; } = new ObservableCollection<FoodDetail>();

    /// <summary>
    /// Khởi tạo một thể hiện mới của lớp <see cref="DiaryViewModel"/>.
    /// </summary>
    /// <param name="localSettingsService">Dịch vụ cài đặt cục bộ.</param>
    /// <param name="apiServicesClient">Dịch vụ API.</param>
    /// <param name="dateTimeProvider">Dịch vụ cung cấp thời gian.</param>
    /// <param name="dialogService">Dịch vụ hộp thoại.</param>
    public DiaryViewModel(ILocalSettingsService localSettingsService, IApiServicesClient apiServicesClient, IDateTimeProvider dateTimeProvider, IDialogService dialogService)
    {
        _localSettingsService = localSettingsService;
        _dialogService = dialogService;
        _apiServicesClient = apiServicesClient;
        _dateTimeProvider = dateTimeProvider;
        SelectedDatesChangedCommand = new RelayCommand<DateTime>(SelectedDatesChanged);
        DecorateDayItemCommand = new RelayCommand<CalendarViewDayItem>(DecorateDayItem);
        InitializeAsync();
        _dialogService = dialogService;
    }

    /// <summary>
    /// Khởi tạo dữ liệu không đồng bộ.
    /// </summary>
    public async void InitializeAsync()
    {
        await GetCustomerId();
        var daySelected = _dateTimeProvider.UtcNow;
        SelectedDayText = _dateTimeProvider.Now.ToString("dd/MM/yyyy");
        await LoadFullData(daySelected);
    }

    /// <summary>
    /// Tải dữ liệu đầy đủ cho ngày được chọn.
    /// </summary>
    /// <param name="daySelected">Ngày được chọn.</param>
    public async Task LoadFullData(DateTime daySelected)
    {
        try
        {
            LoadDailyDiary(daySelected);
        }
        catch(Exception ex)
        {
            Debug.WriteLine("Lỗi hệ thống: " + ex.Message);
            await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: " + ex.Message);
        }

        LoadWorkoudetails(daySelected);
    }

    /// <summary>
    /// Lấy ID khách hàng.
    /// </summary>
    public async Task GetCustomerId()
    {
        var customerId = await _localSettingsService.ReadSettingAsync<string>("customer_id");
        _customer_id = customerId ?? string.Empty;
    }

    /// <summary>
    /// Tải dữ liệu nhật ký hàng ngày.
    /// </summary>
    /// <param name="daySelected">Ngày được chọn.</param>
    public void LoadDailyDiary(DateTime daySelected)
    {
        var CurrentDailydiary = _apiServicesClient.Get<Dailydiary>($"api/Dailydiary/get/daily_customer/{_customer_id}/day/{daySelected:yyyy-MM-dd HH:mm:ss}");

        BreakfastItems.Clear();
        LunchItems.Clear();
        DinnerItems.Clear();
        SnackItems.Clear();

        if (CurrentDailydiary != null)
        {
            foreach (var foodDetail in CurrentDailydiary.Fooddetails)
            {
                switch (foodDetail.meal_type)
                {
                    case 1:
                        BreakfastItems.Add(foodDetail);
                        break;
                    case 2:
                        LunchItems.Add(foodDetail);
                        break;
                    case 3:
                        DinnerItems.Add(foodDetail);
                        break;
                    case 4:
                        SnackItems.Add(foodDetail);
                        break;
                }
            }
            WeightText = CurrentDailydiary.daily_weight;
            BurnedCalories = (int)(CurrentDailydiary.total_calories - CurrentDailydiary.calories_remain);
            TotalCalories = (int)CurrentDailydiary.total_calories;

            IsBreakfastEmpty = BreakfastItems.Count() == 0 ? true : false;
            IsLunchEmpty = LunchItems.Count() == 0 ? true : false;
            IsSnackEmpty = SnackItems.Count() == 0 ? true : false;
            IsDinnerEmpty = DinnerItems.Count() == 0 ? true : false;
        }
        else
        {
            WeightText = 0;
            BurnedCalories = 0;
            TotalCalories = 0;
            IsBreakfastEmpty = true;
            IsLunchEmpty = true;
            IsSnackEmpty = true;
            IsDinnerEmpty = true;
        }
    }


    /// <summary>
    /// Tải chi tiết kế hoạch tập luyện.
    /// </summary>
    /// <param name="daySelected">Ngày được chọn.</param>
    public void LoadWorkoudetails(DateTime daySelected)
    {
        var planDetail = _apiServicesClient.Get<Plandetail>($"api/Plandetail/get/plandetail/{_customer_id}");

        PlanNameText = "Chưa có kế hoạch";
        TypeWorkoutText = "Chưa có ngày tập";
        _exerciseStatus = 0;
        // exercise_status =  0: chưa có plan, 1: ngày nghỉ, 2: ngày tập hoàn thành, 3: ngày tập chưa hoàn thành
        if (planDetail != null)
        {
            var workoutDetailsList = planDetail?.Workoutdetails.ToList();
            var selectedWorkoutDetails = workoutDetailsList?.FirstOrDefault(w => w.date_workout.Date == daySelected.Date);
            if (selectedWorkoutDetails != null) // có thông tin workout -> ngày tập
            {
                PlanNameText = planDetail.Plan.plan_name;
                TypeWorkoutText = selectedWorkoutDetails.Typeworkout.description;
                _exerciseStatus = selectedWorkoutDetails.description == "Đã hoàn thành Exercise trong ngày" ? 2 : 3;
            }
            else if (selectedWorkoutDetails == null && daySelected <= planDetail.end_date && daySelected >= planDetail.start_date) // ngày nghỉ trong kế hoạch
            {
                PlanNameText = planDetail.Plan.plan_name;
                TypeWorkoutText = "Ngày nghỉ";
                _exerciseStatus = 1;
            }
        }
    }

    /// <summary>
    /// Phương thức được gọi khi ngày được chọn thay đổi.
    /// </summary>
    /// <param name="daySelected">Ngày được chọn.</param>
    public void SelectedDatesChanged(DateTime daySelected)
    {
        SelectedDayText = daySelected.ToString("dd/MM/yyyy");
        LoadFullData(daySelected);
    }

    /// <summary>
    /// Trang trí mục ngày trong CalendarView.
    /// </summary>
    /// <param name="dayItem">Mục ngày trong CalendarView.</param>
    public void DecorateDayItem(CalendarViewDayItem dayItem)
    {
        if (dayItem == null) return;

        // Lấy ngày từ dayItem
        DateTime date = dayItem.Date.DateTime.ToUniversalTime();

        // Kiểm tra nếu ngày nằm trong phạm vi
        var startDate = _dateTimeProvider.Now.AddDays(-14);
        var endDate = _dateTimeProvider.Now.AddDays(14);

        if (date >= startDate && date <= endDate)
        {
            LoadWorkoudetails(date);
            switch (_exerciseStatus)
            {
                case 1: // Ngày nghỉ
                    AddColorToDay(dayItem, Windows.UI.Color.FromArgb(255, 173, 216, 230)); // LightBlue
                    break;

                case 2: // Đã hoàn thành
                    AddColorToDay(dayItem, Windows.UI.Color.FromArgb(255, 144, 238, 144)); // LightGreen
                    break;

                case 3: // Chưa hoàn thành
                    AddColorToDay(dayItem, Windows.UI.Color.FromArgb(255, 240, 128, 128)); // LightCoral
                    break;

                default: // Không trạng thái
                    break;
            }
        }
        var daySelected = _dateTimeProvider.UtcNow; // Lấy ngày theo múi giờ địa phương hiện tại
        LoadWorkoudetails(daySelected);
    }

    /// <summary>
    /// Thêm màu cho các ngày tùy theo trạng thái.
    /// </summary>
    /// <param name="dayItem">Ngày được trang trí.</param>
    /// <param name="backgroundColor">Màu tương ứng.</param>
    private void AddColorToDay(CalendarViewDayItem dayItem, Windows.UI.Color backgroundColor)
    {
        if(dayItem.Date > DateTime.Now.Date) { return; }
        dayItem.Background = new SolidColorBrush(backgroundColor);
    }
}