﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Gyminize.Contracts.Services;
using Gyminize.Core.Services;
using Microsoft.UI.Xaml.Controls;
using System.Reflection.Metadata;
using System.Diagnostics;
using System.Net;

namespace Gyminize.ViewModels
{
    public partial class NutritionsViewModel : ObservableObject
    {
        public ILocalSettingsService LocalSetting
        {
            get;
        }
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        public Dailydiary CurrentDailydiary
        {
            get; set;
        }

        public ObservableCollection<FoodDetail> BreakfastItems { get; set; } = new ObservableCollection<FoodDetail>();
        public ObservableCollection<FoodDetail> LunchItems { get; set; } = new ObservableCollection<FoodDetail>();
        public ObservableCollection<FoodDetail> DinnerItems { get; set; } = new ObservableCollection<FoodDetail>();
        public ObservableCollection<FoodDetail> SnackItems { get; set; } = new ObservableCollection<FoodDetail>();

        public ObservableCollection<Food> FoodLibraryItems { get; set; } = new ObservableCollection<Food>();

        private string _totalCaloriesExpression;
        public string TotalCaloriesExpression
        {
            get => _totalCaloriesExpression;
            set => SetProperty(ref _totalCaloriesExpression, value);
        }
        public string CustomerId;
        public ICommand DeleteFoodFromMealCommand
        {
            get;
        }
        public ICommand AddFoodToMealCommand
        {
            get;
        }

        public NutritionsViewModel(INavigationService navigationService,IDialogService dialogService ,ILocalSettingsService localSetting)
        {
            _navigationService = navigationService;
            LocalSetting = localSetting;
            _dialogService = dialogService;
            DeleteFoodFromMealCommand = new AsyncRelayCommand<FoodDetail>(DeleteFoodFromMealAsync);
            AddFoodToMealCommand = new AsyncRelayCommand<Food?>(AddFoodToMealAsync); // Sửa lại dòng này

            LoadFoodLibraryAsync();
            LoadDailyDiary();
            
        }

        private async Task LoadFoodLibraryAsync()
        {
            try
            {
                var foods = ApiServices.Get<List<Food>>("api/Food");
                if (foods != null && foods.Any())
                {
                    FoodLibraryItems.Clear();
                    foreach (var food in foods)
                    {
                        FoodLibraryItems.Add(food);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading food library: {ex.Message}");
            }
        }

        private async Task LoadDailyDiary()
        {
            try
            {
                 CustomerId = await LocalSetting.ReadSettingAsync<string>("customer_id");
                DateTime day = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);
                CurrentDailydiary = ApiServices.Get<Dailydiary>($"api/Dailydiary/get/daily_customer/{CustomerId}/day/{day:yyyy-MM-dd HH:mm:ss}");



                if (CurrentDailydiary != null)
                {
                    BreakfastItems.Clear();
                    LunchItems.Clear();
                    DinnerItems.Clear();
                    SnackItems.Clear();

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

                    UpdateTotalCaloriesExpression();
                }
                else
                {
                    var endpoint = $"api/Customerhealth/get/" + CustomerId;
                    var _customerHealth = ApiServices.Get<CustomerHealth>(endpoint);
                    Dailydiary newDailydiary = new Dailydiary();
                    newDailydiary.customer_id = _customerHealth.customer_id;
                    newDailydiary.diary_date = day;
                    newDailydiary.daily_weight = _customerHealth.weight;
                    newDailydiary.calories_remain = Convert.ToInt32(_customerHealth.tdee);
                    newDailydiary.total_calories = Convert.ToInt32(_customerHealth.tdee);
                    newDailydiary.notes = "nothing";
                    var newDailyDiary = ApiServices.Post<Dailydiary>("api/Dailydiary/create", newDailydiary);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading daily diary: {ex.Message}");
            }
        }

        private void UpdateTotalCaloriesExpression()
        {
            int totalCalories = BreakfastItems.Sum(item => item.TotalCalories) +
                                LunchItems.Sum(item => item.TotalCalories) +
                                DinnerItems.Sum(item => item.TotalCalories) +
                                SnackItems.Sum(item => item.TotalCalories);

            TotalCaloriesExpression = $"{CurrentDailydiary.total_calories:F0} - {totalCalories} = {CurrentDailydiary.total_calories - totalCalories:F0}";
        }



        private async Task AddFoodToMealAsync(Food? selectedFood)
        {
            if (selectedFood == null)
                return;

            // Hiển thị dialog để chọn bữa ăn và số lượng
            var (selectedMeal, quantity) = await _dialogService.ShowMealSelectionDialogAsync();

            if (!string.IsNullOrEmpty(selectedMeal))
            {
                // Tạo đối tượng FoodDetail với các thông tin cần thiết
                var foodDetail = new FoodDetail
                {
                    dailydiary_id = CurrentDailydiary.dailydiary_id,
                    meal_type = selectedMeal switch
                    {
                        "Bữa Sáng" => 1,
                        "Bữa Trưa" => 2,
                        "Bữa Tối" => 3,
                        "Bữa Xế" => 4,
                        _ => 0
                    },
                    food_amount = quantity,
                    Food = selectedFood
                };

                // Gọi API để thêm hoặc cập nhật FoodDetail
                try
                {
                    var updateResult = ApiServices.Put<FoodDetail>("api/foodetail/update", foodDetail);

                    if (updateResult != null)
                    {
                        System.Diagnostics.Debug.WriteLine("FoodDetail added/updated successfully.");
                        LoadDailyDiary();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to add/update FoodDetail.");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error updating FoodDetail: {ex.Message}");
                }

                // Cập nhật biểu thức tổng calo
                UpdateTotalCaloriesExpression();
            }
        }



        private async Task DeleteFoodFromMealAsync(FoodDetail foodDetail)
        {
            if (foodDetail == null)
                return;

            // Tạo đối tượng với thông tin cần thiết để xóa
            

            // Gọi API để xóa FoodDetail
            try
            {
         
                var deleteResult = ApiServices.Delete($"api/Foodetail/delete", foodDetail);

                if (deleteResult)
                {
                    System.Diagnostics.Debug.WriteLine("FoodDetail deleted successfully.");
                    
                    // Xóa khỏi ObservableCollection trong ViewModel
                    if (BreakfastItems.Contains(foodDetail))
                    {
                        BreakfastItems.Remove(foodDetail);
                    }
                    else if (LunchItems.Contains(foodDetail))
                    {
                        LunchItems.Remove(foodDetail);
                    }
                    else if (DinnerItems.Contains(foodDetail))
                    {
                        DinnerItems.Remove(foodDetail);
                    }
                    else if (SnackItems.Contains(foodDetail))
                    {
                        SnackItems.Remove(foodDetail);
                    }

                    // Cập nhật biểu thức tổng calo
                    UpdateTotalCaloriesExpression();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Failed to delete FoodDetail.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting FoodDetail: {ex.Message}");
            }
        }

    }
}
