// ViewModel cho trang dinh dưỡng.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Gyminize.Models;

namespace Gyminize.ViewModels;

public partial class NutritionsViewModel : ObservableRecipient
{
    private ObservableCollection<FoodItem> breakfastItems = new ObservableCollection<FoodItem>();
    private ObservableCollection<FoodItem> lunchItems = new ObservableCollection<FoodItem>();
    private ObservableCollection<FoodItem> dinnerItems = new ObservableCollection<FoodItem>();
    private ObservableCollection<FoodItem> snackItems = new ObservableCollection<FoodItem>();
    private ObservableCollection<FoodItem> foodLibraryItems = new ObservableCollection<FoodItem>();

    public ObservableCollection<FoodItem> BreakfastItems
    {
        get => breakfastItems;
        set => SetProperty(ref breakfastItems, value);
    }

    public ObservableCollection<FoodItem> LunchItems
    {
        get => lunchItems;
        set => SetProperty(ref lunchItems, value);
    }

    public ObservableCollection<FoodItem> DinnerItems
    {
        get => dinnerItems;
        set => SetProperty(ref dinnerItems, value);
    }

    public ObservableCollection<FoodItem> SnackItems
    {
        get => snackItems;
        set => SetProperty(ref snackItems, value);
    }

    public ObservableCollection<FoodItem> FoodLibraryItems
    {
        get => foodLibraryItems;
        set => SetProperty(ref foodLibraryItems, value);
    }

    public string TotalCaloriesExpression
    {
        get
        {
            int totalCalories = BreakfastItems.Sum(item => item.Calories) +
                                LunchItems.Sum(item => item.Calories) +
                                DinnerItems.Sum(item => item.Calories) +
                                SnackItems.Sum(item => item.Calories);
            return $"2000 - {totalCalories} = {2000 - totalCalories}";
        }
    }

    public NutritionsViewModel()
    {
        LoadSampleData();
        BreakfastItems.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalCaloriesExpression));
        LunchItems.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalCaloriesExpression));
        DinnerItems.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalCaloriesExpression));
        SnackItems.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalCaloriesExpression));
    }

    private void LoadSampleData()
    {
        var sampleDataService = new SampleFoodDataService();
        this.FoodLibraryItems = sampleDataService.GetSampleFoodData();
    }
}

