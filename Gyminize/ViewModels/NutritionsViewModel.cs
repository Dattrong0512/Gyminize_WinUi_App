using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Gyminize.Contracts.Services;

using Microsoft.UI.Xaml.Controls;
using System.Reflection.Metadata;
using System.Diagnostics;
using System.Net;

namespace Gyminize.ViewModels;

/// <summary>
/// ViewModel cho màn hình dinh dưỡng (Nutritions).
/// </summary>
/// <remarks>
/// Lớp này chịu trách nhiệm quản lý các dữ liệu và logic liên quan đến dinh dưỡng của người dùng, 
/// bao gồm việc quản lý các món ăn trong nhật ký hàng ngày và thư viện thực phẩm. 
/// Nó cũng cung cấp các lệnh (command) để thao tác với dữ liệu, 
/// chẳng hạn như thêm, xóa món ăn, và tìm kiếm thực phẩm trong thư viện.
/// </remarks>
public partial class NutritionsViewModel : ObservableObject
{
    public ILocalSettingsService LocalSetting
    {
        get;
    }
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IApiServicesClient _apiServicesClient;
    public Dailydiary CurrentDailydiary
    {
        get; set;
    }

    public ObservableCollection<FoodDetail> BreakfastItems { get; set; } = new ObservableCollection<FoodDetail>();
    public ObservableCollection<FoodDetail> LunchItems { get; set; } = new ObservableCollection<FoodDetail>();
    public ObservableCollection<FoodDetail> DinnerItems { get; set; } = new ObservableCollection<FoodDetail>();
    public ObservableCollection<FoodDetail> SnackItems { get; set; } = new ObservableCollection<FoodDetail>();

    public ObservableCollection<Food> FoodLibraryItems { get; set; } = new ObservableCollection<Food>();
    public ObservableCollection<Food> FilteredFoodLibraryItems { get; set; } = new ObservableCollection<Food>();
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
    public ICommand SearchFoodCommand
    {
        get;
    }

    /// <summary>
    /// Hàm khởi tạo cho ViewModel dinh dưỡng.
    /// </summary>
    /// <param name="navigationService">Dịch vụ điều hướng để chuyển màn hình.</param>
    /// <param name="dialogService">Dịch vụ quản lý hộp thoại.</param>
    /// <param name="localSetting">Dịch vụ lưu trữ cài đặt cục bộ của ứng dụng.</param>
    /// <param name="apiServicesClient">Dịch vụ kết nối API để lấy và gửi dữ liệu từ/đến máy chủ.</param>
    /// <remarks>
    /// Hàm khởi tạo này thiết lập các dịch vụ cần thiết cho ViewModel, bao gồm điều hướng, 
    /// hộp thoại và các dịch vụ API. Nó cũng khởi tạo các lệnh (commands) cho các chức năng 
    /// như thêm thực phẩm vào bữa ăn, xóa thực phẩm và tìm kiếm thực phẩm.
    /// </remarks>
    public NutritionsViewModel(INavigationService navigationService,IDialogService dialogService ,ILocalSettingsService localSetting, IApiServicesClient apiServicesClient)
    {
        _apiServicesClient = apiServicesClient;
        _navigationService = navigationService;
        LocalSetting = localSetting;
        _dialogService = dialogService;
        DeleteFoodFromMealCommand = new AsyncRelayCommand<FoodDetail>(DeleteFoodFromMealAsync);
        AddFoodToMealCommand = new AsyncRelayCommand<Food?>(AddFoodToMealAsync); // Sửa lại dòng này
        SearchFoodCommand = new RelayCommand<string>(SearchFoodLibrary);
        LoadFoodLibraryAsync();
        LoadDailyDiary();
        
    }
    /// <summary>
    /// Tải thư viện thực phẩm từ API và cập nhật danh sách thực phẩm trong ViewModel.
    /// </summary>
    /// <remarks>
    /// Hàm này gửi yêu cầu đến API để lấy danh sách các thực phẩm và thêm vào `FoodLibraryItems`. 
    /// Sau đó, danh sách này được sao chép vào `FilteredFoodLibraryItems` để sử dụng cho việc tìm kiếm thực phẩm.
    /// </remarks>
    public async Task LoadFoodLibraryAsync()
    {
        try
        {
            var foods = _apiServicesClient.Get<List<Food>>("api/Food");
            if (foods != null && foods.Any())
            {
                FoodLibraryItems.Clear();
                foreach (var food in foods)
                {
                    FoodLibraryItems.Add(food);
                }
            }
            FilteredFoodLibraryItems = new ObservableCollection<Food>(FoodLibraryItems);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading food library: {ex.Message}");
            await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: " + ex.Message);
        }
    }

    /// <summary>
    /// Tìm kiếm thực phẩm trong thư viện thực phẩm theo từ khóa.
    /// </summary>
    /// <param name="searchText">Từ khóa tìm kiếm.</param>
    /// <remarks>
    /// Hàm này lọc danh sách thực phẩm dựa trên từ khóa tìm kiếm và cập nhật `FilteredFoodLibraryItems`.
    /// Nếu không có từ khóa, tất cả thực phẩm sẽ được hiển thị.
    /// </remarks>
    public void SearchFoodLibrary(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            FilteredFoodLibraryItems.Clear();
            foreach (var item in FoodLibraryItems)
            {
                FilteredFoodLibraryItems.Add(item);
            }
        }
        else
        {
            var filteredItems = FoodLibraryItems.Where(f => f.food_name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
            FilteredFoodLibraryItems.Clear();
            foreach (var item in filteredItems)
            {
                FilteredFoodLibraryItems.Add(item);
            }
        }
    }

    /// <summary>
    /// Tải nhật ký hàng ngày của người dùng từ API và cập nhật các bữa ăn.
    /// </summary>
    /// <remarks>
    /// Hàm này lấy thông tin nhật ký hàng ngày của người dùng từ API, phân loại các thực phẩm theo từng bữa ăn 
    /// (bữa sáng, bữa trưa, bữa tối, bữa xế) và cập nhật các danh sách thực phẩm tương ứng.
    /// </remarks>
    public async Task LoadDailyDiary()
    {
        try
        {
            CustomerId = await LocalSetting.ReadSettingAsync<string>("customer_id");
            var utcNow = DateTime.UtcNow;
            var localTimeZone = TimeZoneInfo.Local;
            var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
            var day = localDateTime; // Lấy ngày theo múi giờ địa phương hiện tại
            CurrentDailydiary = _apiServicesClient.Get<Dailydiary>($"api/Dailydiary/get/daily_customer/{CustomerId}/day/{day:yyyy-MM-dd HH:mm:ss}");
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
            await _dialogService.ShowErrorDialogAsync($"Lỗi hệ thống:  {ex.Message}");
        }
    }

    /// <summary>
    /// Cập nhật biểu thức tính tổng số calo tiêu thụ từ các bữa ăn.
    /// </summary>
    /// <remarks>
    /// Hàm này tính tổng số calo từ các thực phẩm trong bữa sáng, bữa trưa, bữa tối và bữa xế, 
    /// sau đó cập nhật biểu thức hiển thị tổng calo tiêu thụ và calo còn lại.
    /// </remarks>
    public void UpdateTotalCaloriesExpression()
    {
        int totalCalories = BreakfastItems.Sum(item => item.TotalCalories) +
                            LunchItems.Sum(item => item.TotalCalories) +
                            DinnerItems.Sum(item => item.TotalCalories) +
                            SnackItems.Sum(item => item.TotalCalories);

        TotalCaloriesExpression = $"{CurrentDailydiary.total_calories:F0} - {totalCalories} = {CurrentDailydiary.total_calories - totalCalories:F0}";
    }


    /// <summary>
    /// Thêm thực phẩm vào bữa ăn trong nhật ký hàng ngày.
    /// </summary>
    /// <param name="selectedFood">Thực phẩm được chọn để thêm vào bữa ăn.</param>
    /// <remarks>
    /// Hàm này hiển thị hộp thoại để chọn bữa ăn và số lượng thực phẩm, 
    /// sau đó gọi API để cập nhật thông tin thực phẩm vào bữa ăn tương ứng.
    /// </remarks>
    public async Task AddFoodToMealAsync(Food? selectedFood)
    {
        if (selectedFood == null)
            return;

        var (selectedMeal, quantity) = await _dialogService.ShowMealSelectionDialogAsync();

        if (!string.IsNullOrEmpty(selectedMeal))
        {
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

            try
            {
                var updateResult = _apiServicesClient.Put<FoodDetail>("api/foodetail/update", foodDetail);

                if (updateResult != null)
                {
                    LoadDailyDiary();
                    await _dialogService.ShowSuccessMessageAsync("Thêm thức ăn thành công");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Failed to add/update FoodDetail.");
                    await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: không thể thêm thức ăn");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating FoodDetail: {ex.Message}");
                await _dialogService.ShowErrorDialogAsync($"Lỗi hệ thống: {ex.Message}");
            }

            // Cập nhật biểu thức tổng calo
            UpdateTotalCaloriesExpression();
        }
    }


    /// <summary>
    /// Xóa thực phẩm khỏi bữa ăn trong nhật ký hàng ngày.
    /// </summary>
    /// <param name="foodDetail">Thông tin chi tiết về thực phẩm cần xóa.</param>
    /// <remarks>
    /// Hàm này gửi yêu cầu đến API để xóa thực phẩm khỏi bữa ăn tương ứng. 
    /// Nếu xóa thành công, thực phẩm sẽ bị loại bỏ khỏi danh sách các món ăn trong bữa sáng, trưa, tối hoặc xế.
    /// Sau đó, hàm cập nhật lại tổng số calo và hiển thị thông báo thành công. 
    /// Nếu có lỗi trong quá trình xóa hoặc API trả về kết quả không thành công, 
    /// một thông báo lỗi sẽ được hiển thị cho người dùng.
    /// </remarks>
    public async Task DeleteFoodFromMealAsync(FoodDetail foodDetail)
    {
        if (foodDetail == null)
            return;
        // Gọi API để xóa FoodDetail
        try
        {
            var deleteResult = _apiServicesClient.Delete($"api/Foodetail/delete", foodDetail);

            if (deleteResult)
            {
                System.Diagnostics.Debug.WriteLine("FoodDetail deleted successfully.");
                
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
                await _dialogService.ShowSuccessMessageAsync("Xóa thức ăn thành công");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to delete FoodDetail.");
                await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: không thể xóa thức ăn");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error deleting FoodDetail: {ex.Message}");
            await _dialogService.ShowErrorDialogAsync($"Lỗi hệ thống: {ex.Message}");
        }
    }
}
