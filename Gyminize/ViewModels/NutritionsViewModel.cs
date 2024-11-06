using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Gyminize.Contracts.Services;
using Gyminize.Core.Services;

namespace Gyminize.ViewModels
{
    public partial class NutritionsViewModel : ObservableObject
    {
        public ILocalSettingsService LocalSetting
        {
            get;
        }
        private readonly INavigationService _navigationService;

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

        public ICommand AddFoodCommand
        {
            get;
        }
        public ICommand DeleteFoodCommand
        {
            get;
        }

        public NutritionsViewModel(INavigationService navigationService, ILocalSettingsService localSetting)
        {
            _navigationService = navigationService;
            LocalSetting = localSetting;

            AddFoodCommand = new RelayCommand<Food>(AddFoodToMeal);
            DeleteFoodCommand = new RelayCommand<FoodDetail>(DeleteFoodFromMeal);

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
                var customerId = await LocalSetting.ReadSettingAsync<string>("customer_id");
                CurrentDailydiary = ApiServices.Get<Dailydiary>($"api/Dailydiary/get/daily_customer/{customerId}");

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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading daily diary: {ex.Message}");
            }
        }

        private void UpdateTotalCaloriesExpression()
        {
            int totalCalories = BreakfastItems.Sum(item => item.Food.calories) +
                                LunchItems.Sum(item => item.Food.calories) +
                                DinnerItems.Sum(item => item.Food.calories) +
                                SnackItems.Sum(item => item.Food.calories);

            TotalCaloriesExpression = $"{CurrentDailydiary.total_calories} - {totalCalories} = {CurrentDailydiary.total_calories - totalCalories}";
        }

        private void AddFoodToMeal(Food food)
        {
            var foodDetail = new FoodDetail
            {
                food_id = food.food_id,
                food_amount = 100, // mặc định số lượng là 100 gram, có thể tuỳ chỉnh
                meal_type = 1, // ví dụ bữa sáng
                Food = food
            };

            BreakfastItems.Add(foodDetail);
            UpdateTotalCaloriesExpression();
        }

        private void DeleteFoodFromMeal(FoodDetail foodDetail)
        {
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

            UpdateTotalCaloriesExpression();
        }
    }
}
