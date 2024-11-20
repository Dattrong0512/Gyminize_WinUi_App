﻿// ViewModel cho trang nhật ký.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using System.Collections.ObjectModel;
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

public partial class DiaryViewModel : ObservableRecipient
{
    // Khởi tạo DiaryViewModel.
    private readonly ILocalSettingsService _localSettingsService;
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

    private int _exerciseStatus;

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

    public DiaryViewModel(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
        SelectedDatesChangedCommand = new RelayCommand<DateTime>(SelectedDatesChanged);
        DecorateDayItemCommand = new RelayCommand<CalendarViewDayItem>(DecorateDayItem);
        InitializeAsync();
    }

    public async void InitializeAsync()
    {
        await GetCustomerId();
        var daySelected = DateTime.UtcNow;
        SelectedDayText = DateTime.Now.ToString("dd/MM/yyyy");
        
        LoadFullData(daySelected);
    }
    public void LoadFullData(DateTime daySelected)
    {
        LoadDailyDiary(daySelected);
        LoadWorkoudetails(daySelected);
    }
    public async Task GetCustomerId()
    {
        var customerId = await _localSettingsService.ReadSettingAsync<string>("customer_id");
        _customer_id = customerId ?? string.Empty;
    }

    public void LoadDailyDiary(DateTime daySelected)
    {
        var CurrentDailydiary = ApiServices.Get<Dailydiary>($"api/Dailydiary/get/daily_customer/{_customer_id}/day/{daySelected:yyyy-MM-dd HH:mm:ss}");

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
        }
    }

    

    public void LoadWorkoudetails(DateTime daySelected)
    {
        var planDetail = ApiServices.Get<Plandetail>($"api/Plandetail/get/plandetail/{_customer_id}");
        PlanNameText = "Chưa có kế hoạch";
        TypeWorkoutText = "Chưa có ngày tập";
        _exerciseStatus = 0; // 0: chưa có plan, 1: ngày nghỉ, 2: ngày tập hoàn thành, 3: ngày tập chưa hoàn thành
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

    public void SelectedDatesChanged(DateTime daySelected)
    {
        SelectedDayText = daySelected.ToString("dd/MM/yyyy");
        LoadFullData(daySelected);
    }

    public void DecorateDayItem(CalendarViewDayItem dayItem)
    {
        if (dayItem == null) return;

        // Lấy ngày từ dayItem
        DateTime date = dayItem.Date.DateTime.ToUniversalTime();

        // Kiểm tra nếu ngày nằm trong phạm vi
        var startDate = DateTime.Now.AddDays(-14);
        var endDate = DateTime.Now.AddDays(14);

        if (date >= startDate && date <= endDate)
        {
            LoadWorkoudetails(date);
            switch (_exerciseStatus)
            {
                case 1: // Ngày nghỉ
                    AddIconToDay(dayItem, Symbol.Accept, Windows.UI.Color.FromArgb(255, 173, 216, 230)); // LightBlue
                    break;

                case 2: // Đã hoàn thành
                    AddIconToDay(dayItem, Symbol.AddFriend, Windows.UI.Color.FromArgb(255, 144, 238, 144)); // LightGreen
                    break;

                case 3: // Chưa hoàn thành
                    AddIconToDay(dayItem, Symbol.Admin, Windows.UI.Color.FromArgb(255, 240, 128, 128)); // LightCoral
                    break;

                default: // Không trạng thái
                    break;
            }
        }
    }

    private void AddIconToDay(CalendarViewDayItem dayItem, Symbol symbol, Windows.UI.Color backgroundColor)
    {
        if(dayItem.Date > DateTime.Now.Date) { return; }
        dayItem.Background = new SolidColorBrush(backgroundColor);

        //var icon = new SymbolIcon(symbol)
        //{
        //    Width = 12,
        //    Height = 12
        //};

        //var container = VisualTreeHelper.GetChild(dayItem, 0) as Grid;
        //if (container != null)
        //{
        //    if (!container.Children.OfType<SymbolIcon>().Any(existingIcon => existingIcon.Symbol == symbol))
        //    {
        //        container.Children.Add(icon);
        //    }
        //}
    }
}