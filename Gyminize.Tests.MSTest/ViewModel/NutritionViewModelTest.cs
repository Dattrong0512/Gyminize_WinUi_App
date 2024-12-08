using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Gyminize.ViewModels;
using Gyminize.Contracts.Services;
using System.Threading.Tasks;
using Gyminize.Core.Services;
using Gyminize.Models;


namespace Gyminize.Tests.MSTest.ViewModel
{
    /// <summary>
    /// Lớp kiểm thử dành cho ViewModel quản lý dinh dưỡng (NutritionsViewModel).
    /// </summary>
    [TestClass] 
    public class NutritionViewModelTest
    {
        private Mock<ILocalSettingsService>? _mockLocalSettingsService;
        private Mock<IDialogService>? _mockDialogService;
        private Mock<INavigationService>? _mockNavigationService;
        private Mock<IApiServicesClient>? _mockApiServicesClient;
        private NutritionsViewModel? _viewModel;

        /// <summary>
        /// Thiết lập môi trường kiểm thử trước mỗi phương thức test case.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockLocalSettingsService = new Mock<ILocalSettingsService>();
            _mockDialogService = new Mock<IDialogService>();
            _mockNavigationService = new Mock<INavigationService>();
            _mockApiServicesClient = new Mock<IApiServicesClient>();

            _viewModel = new NutritionsViewModel(
                _mockNavigationService.Object,
                _mockDialogService.Object,
                _mockLocalSettingsService.Object,
                _mockApiServicesClient.Object
            );
        }

        /// <summary>
        /// Kiểm tra việc tải danh sách thức ăn từ nhật ký dinh dưỡng (daily diary) khi API trả về dữ liệu hợp lệ.
        /// </summary>
        [TestMethod] 
        public async Task LoadDailyDiary_ShouldPopulateItems_WhenApiReturnsData()
        {
            // Arrange
            _mockLocalSettingsService!
                .Setup(s => s.ReadSettingAsync<string>("customer_id"))
                .ReturnsAsync("12345");

            _mockApiServicesClient!
                .Setup(client => client.Get<Dailydiary>(It.IsAny<string>()))
                .Returns(new Dailydiary
                {
                    Fooddetails = new List<FoodDetail>
                    {
                        new FoodDetail { meal_type = 1,  Food = new Food { calories = 150 }, food_amount = 2 },
                        new FoodDetail { meal_type = 2, Food = new Food { calories = 100 }, food_amount = 1 }
                    },
                    total_calories = 2000
                });

            // Act
            await _viewModel!.LoadDailyDiary();

            // Assert(đảm bảo các item cho buổi sáng và trưa được thêm vào như mock data và TotalCaloriesExpression được tính toán chính xác)
            Assert.AreEqual(1, _viewModel.BreakfastItems.Count);
            Assert.AreEqual(1, _viewModel.LunchItems.Count);
            Assert.AreEqual(0, _viewModel.DinnerItems.Count);
            Assert.AreEqual(0, _viewModel.SnackItems.Count);
            Assert.AreEqual("2000 - 400 = 1600", _viewModel.TotalCaloriesExpression);
        }

        /// <summary>
        /// Kiểm tra việc hiển thị thông báo lỗi khi tải danh sách thức ăn từ nhật ký dinh dưỡng (daily diary) gặp lỗi API.
        /// </summary>
        [TestMethod]
        public async Task LoadDailyDiary_ShouldShowErrorDialog_WhenApiThrowsException()
        {
            // Arrange
            _mockLocalSettingsService!
                .Setup(s => s.ReadSettingAsync<string>("customer_id"))
                .ReturnsAsync("12345");

            _mockApiServicesClient!
                .Setup(client => client.Get<Dailydiary>(It.IsAny<string>()))
                .Throws(new Exception("API error"));

            _mockDialogService!
                .Setup(d => d.ShowErrorDialogAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _viewModel!.LoadDailyDiary();

            // Assert(kiểm tra xem dich vụ xuất dialog lỗi có hoạt động và đúng message)
            _mockDialogService.Verify(
                d => d.ShowErrorDialogAsync(It.Is<string>(msg => msg.Contains("API error"))),
                Times.Once
            );
        }

        /// <summary>
        /// Kiểm tra việc tải thư viện thức ăn khi API trả về dữ liệu hợp lệ.
        /// </summary>
        [TestMethod]
        public async Task LoadFoodLibrary_ShouldPopulateItems_WhenApiReturnsData()
        {
            // Arrange
            _mockApiServicesClient!
                .Setup(client => client.Get<List<Food>>(It.IsAny<string>()))
                .Returns(new List<Food>
                {
                    new Food { calories = 100, food_name = "Apple" },
                    new Food { calories = 200, food_name = "Banana" }
                });

            // Act
            await _viewModel!.LoadFoodLibraryAsync();

            // Assert (Đảm bảo có 2 item như đã tạo và item đầu là Apple )
            Assert.AreEqual(2, _viewModel.FoodLibraryItems.Count);
            Assert.AreEqual("Apple", _viewModel.FoodLibraryItems[0].food_name);
        }

        /// <summary>
        /// Kiểm tra việc thêm thức ăn vào bữa ăn khi API trả về dữ liệu hợp lệ.
        /// </summary>
        [TestMethod] 
        public async Task AddFoodToMeal_ShouldCallApiAndUpdateData_WhenValidMealIsSelected()
        {
            // Arrange
            var selectedFood = new Food { calories = 100, food_name = "Apple" };
            var mockDiary = new Dailydiary { dailydiary_id = 1 };
            _viewModel!.CurrentDailydiary = mockDiary;

            _mockDialogService!
                .Setup(d => d.ShowMealSelectionDialogAsync())
                .ReturnsAsync(("Bữa Sáng", 2));

            _mockApiServicesClient!
                .Setup(client => client.Put<FoodDetail>(It.IsAny<string>(), It.IsAny<FoodDetail>()))
                .Returns(new FoodDetail { meal_type = 1, food_amount = 2, Food = selectedFood });

            _mockApiServicesClient!
                .Setup(client => client.Get<Dailydiary>(It.IsAny<string>()))
                .Returns(mockDiary);

            // Act
            await _viewModel.AddFoodToMealAsync(selectedFood);

            // Assert (Đảm bảo put được gọi (cập nhật) và get được gọi (reload))
            _mockApiServicesClient.Verify(client =>
                client.Put<FoodDetail>(It.IsAny<string>(), It.IsAny<FoodDetail>()), Times.Once);

            _mockApiServicesClient.Verify(client =>
                client.Get<Dailydiary>(It.IsAny<string>()), Times.Exactly(2));
        }

        /// <summary>
        /// Kiểm tra việc hiển thị thông báo lỗi khi thêm thức ăn vào bữa ăn và API gặp lỗi.
        /// </summary>
        [TestMethod] 
        public async Task AddFoodToMeal_ShouldShowErrorDialog_WhenApiPutThrowsException()
        {
            // Arrange
            var selectedFood = new Food { calories = 100, food_name = "Apple" };
            var mockDiary = new Dailydiary { dailydiary_id = 1 };
            _viewModel!.CurrentDailydiary = mockDiary;

            _mockDialogService!
                .Setup(d => d.ShowErrorDialogAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _mockApiServicesClient!
                .Setup(client => client.Put<FoodDetail>(It.IsAny<string>(), It.IsAny<FoodDetail>()))
                .Throws(new Exception("API error during PUT"));

            _mockDialogService!
                .Setup(d => d.ShowMealSelectionDialogAsync())
                .ReturnsAsync(("Bữa Sáng", 2));

            // Act
            await _viewModel.AddFoodToMealAsync(selectedFood);

            // Assert (Kiểm tra có message lỗi và gọi đến api put 1 lần)
            _mockDialogService.Verify(d =>
                d.ShowErrorDialogAsync(It.Is<string>(msg => msg.Contains("API error during PUT"))),
                Times.Once);

            _mockApiServicesClient.Verify(client =>
                client.Put<FoodDetail>(It.IsAny<string>(), It.IsAny<FoodDetail>()), Times.Once);
        }

        /// <summary>
        /// Kiểm tra việc xóa thức ăn khỏi bữa ăn khi API thành công.
        /// </summary>
        [TestMethod]
        public async Task DeleteFoodFromMeal_ShouldRemoveItemFromList_WhenApiCallSucceeds()
        {
            // Arrange
            var foodDetail = new FoodDetail { meal_type = 1, Food = new Food { calories = 100 }, food_amount = 1 };
            _viewModel!.BreakfastItems.Add(foodDetail);

            _mockApiServicesClient!
                .Setup(client => client.Delete(It.IsAny<string>(), foodDetail))
                .Returns(true);

            // Act
            await _viewModel.DeleteFoodFromMealAsync(foodDetail);

            // Assert (đảm bảo item buổi sáng (meal_type = 1) đã bị xóa)
            Assert.AreEqual(0, _viewModel.BreakfastItems.Count);
        }

        /// <summary>
        /// Kiểm tra việc hiển thị thông báo lỗi khi xóa thức ăn khỏi bữa ăn và API gặp lỗi.
        /// </summary>
        [TestMethod] 
        public async Task DeleteFoodFromMeal_ShouldShowErrorDialog_WhenApiDeleteThrowsException()
        {
            // Arrange
            var foodDetail = new FoodDetail { meal_type = 1, Food = new Food { calories = 100 }, food_amount = 1 };

            _mockDialogService!
                .Setup(d => d.ShowErrorDialogAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _mockApiServicesClient!
                .Setup(client => client.Delete(It.IsAny<string>(), It.IsAny<FoodDetail>()))
                .Throws(new Exception("API error during DELETE"));

            // Act
            await _viewModel!.DeleteFoodFromMealAsync(foodDetail);

            // Assert (Kiểm tra có message lỗi và gọi đến api delete 1 lần)
            _mockDialogService.Verify(d =>
                d.ShowErrorDialogAsync(It.Is<string>(msg => msg.Contains("API error during DELETE"))),
                Times.Once);

            _mockApiServicesClient.Verify(client =>
                client.Delete(It.IsAny<string>(), It.IsAny<FoodDetail>()), Times.Once);
        }
    }
}
