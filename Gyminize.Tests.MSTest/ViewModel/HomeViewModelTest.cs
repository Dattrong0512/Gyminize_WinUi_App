using System;
using System.Threading.Tasks;
using Gyminize.Contracts.Services;
using Gyminize.ViewModels;
using Gyminize.Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.UI.Xaml;


namespace Gyminize.Tests.MSTest.ViewModel;
[TestClass]
public class HomeViewModelTest
{
    private Mock<ILocalSettingsService>? _mockLocalSettingsService;
    private Mock<IDialogService>? _mockDialogService;
    private Mock<INavigationService>? _mockNavigationService;
    private Mock<IApiServicesClient>? _mockApiServicesClient;
    private Mock<IWindowService>? _mockWindowService;
    private Mock<IDateTimeProvider> _mockDateTimeProvider;
    private HomeViewModel? _viewModel;

    [TestInitialize]
    public void Setup()
    {
        _mockLocalSettingsService = new Mock<ILocalSettingsService>();
        _mockDialogService = new Mock<IDialogService>();
        _mockNavigationService = new Mock<INavigationService>();
        _mockApiServicesClient = new Mock<IApiServicesClient>();
        _mockWindowService = new Mock<IWindowService>();
        _mockDateTimeProvider = new Mock<IDateTimeProvider>();

        _viewModel = new HomeViewModel(
            _mockNavigationService.Object,
            _mockWindowService.Object,
            _mockLocalSettingsService.Object,
            _mockDialogService.Object,
            _mockApiServicesClient.Object,
            _mockDateTimeProvider.Object
            );
    }

    [TestMethod] //Test case kiểm tra báo lỗi cân nặng nhập vào không phải là một số
    public async Task OpenSaveWeight_ShouldShowErrorDialog_WhenWeightIsNotNumeric()
    {
        // Arrange
        _viewModel.WeightText = "abc";

        // Act
        _viewModel.OpenSaveWeight();

        // Assert
        _mockDialogService.Verify(ds => ds.ShowErrorDialogAsync("Lỗi: Cân nặng phải là một số"), Times.Once);
    }

    [TestMethod] //Test case kiểm tra báo lỗi khi cân nặng nhập vào vượt miền giá trị cho phép
    public async Task OpenSaveWeight_ShouldShowErrorDialog_WhenWeightIsOutOfRange()
    {
        // Arrange
        _viewModel.WeightText = "250";

        // Act
        _viewModel.OpenSaveWeight();

        // Assert
        _mockDialogService.Verify(ds => ds.ShowErrorDialogAsync("Lỗi: Ứng dụng chỉ hỗ trợ cân nặng từ 30kg đến 200kg"), Times.Once);
    }

    [TestMethod] //Test case kiểm tra khi cân nặng nhập vào hợp lệ
    public async Task OpenSaveWeight_ShouldUpdateWeight_WhenWeightIsValid()
    {
        // Arrange
        _viewModel.WeightText = "70";
        _viewModel._customer_id = "123";
        _mockApiServicesClient.Setup(api => api.Put<CustomerHealth>(It.IsAny<string>(), null)).Returns(new CustomerHealth());

        // Act
        _viewModel.OpenSaveWeight();

        // Assert
        _mockApiServicesClient.Verify(api => api.Put<CustomerHealth>($"api/Customerhealth/update/123/weight/70", null), Times.Once);
        Assert.AreEqual("70", _viewModel.WeightText);
        Assert.IsFalse(_viewModel.IsWeightTextBoxEnabled);
    }

    [TestMethod]//Test case kiểm tra báo lỗi khi cân nặng nhập vào hợp lệ nhưng bị lỗi api
    public async Task OpenSaveWeight_ShouldShowErrorDialog_WhenApiThrowsException()
    {
        // Arrange
        _viewModel.WeightText = "70";
        _viewModel._customer_id = "123";
        _mockApiServicesClient.Setup(api => api.Put<CustomerHealth>(It.IsAny<string>(), null)).Throws(new Exception("API error"));

        // Act
        _viewModel.OpenSaveWeight();

        // Assert
        _mockDialogService.Verify(ds => ds.ShowErrorDialogAsync("Lỗi hệ thống: API error"), Times.Once);
    }

    [TestMethod]// Test case kiểm tra dữ liệu tiến độ calo tiêu thụ trong ngày hiển thị khi có dữ liệu diary hiện hành và không vượt mức
    public async Task OnNavigatedTo_CurrentDailydiaryNotNull_ProgressValueLessThanOrEqualTo100()
    {
        // Arrange
        var customerId = "123";
        var currentDailydiary = new Dailydiary
        {
            daily_weight = 70,
            total_calories = 2000,
            calories_remain = 500
        };
        _mockLocalSettingsService.Setup(s => s.ReadSettingAsync<string>("customer_id")).ReturnsAsync(customerId);
        _mockApiServicesClient.Setup(c => c.Get<CustomerHealth>(It.IsAny<string>())).Returns(new CustomerHealth());
        _mockApiServicesClient.Setup(c => c.Get<Dailydiary>(It.IsAny<string>())).Returns(currentDailydiary);
        _mockDateTimeProvider.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

        // Act
        await Task.Run(() => _viewModel.OnNavigatedTo(null));

        // Assert
        Assert.AreEqual("70", _viewModel.WeightText);
        Assert.AreEqual("2000", _viewModel.GoalCalories);
        Assert.AreEqual("500", _viewModel.RemainCalories);
        Assert.AreEqual("1500", _viewModel.BurnedCalories);
        Assert.AreEqual(75, _viewModel.ProgressValue);
        Assert.AreEqual(false, _viewModel.IsOverGoalCalories);
    }

    [TestMethod]// Test case kiểm tra dữ liệu tiến độ calo tiêu thụ trong ngày hiển thị khi có dữ liệu diary hiện hành và vượt mức
    public async Task OnNavigatedTo_CurrentDailydiaryNotNull_ProgressValueGreaterThan100()
    {
        // Arrange
        var customerId = "123";
        var currentDailydiary = new Dailydiary
        {
            daily_weight = 70,
            total_calories = 2000,
            calories_remain = -100
        };
        _mockLocalSettingsService.Setup(s => s.ReadSettingAsync<string>("customer_id")).ReturnsAsync(customerId);
        _mockApiServicesClient.Setup(c => c.Get<CustomerHealth>(It.IsAny<string>())).Returns(new CustomerHealth());
        _mockApiServicesClient.Setup(c => c.Get<Dailydiary>(It.IsAny<string>())).Returns(currentDailydiary);
        _mockDateTimeProvider.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

        // Act
        await Task.Run(() => _viewModel.OnNavigatedTo(null));

        // Assert
        Assert.AreEqual("70", _viewModel.WeightText);
        Assert.AreEqual("2000", _viewModel.GoalCalories);
        Assert.AreEqual("-100", _viewModel.RemainCalories);
        Assert.AreEqual("2100", _viewModel.BurnedCalories);
        Assert.AreEqual(105, _viewModel.ProgressValue);
        Assert.AreEqual(true, _viewModel.IsOverGoalCalories);
    }

    [TestMethod] // Test case kiểm tra trường hợp chưa có dữ liệu dailydiary hiện hành cần tạo mới một dailydiary
    public async Task OnNavigatedTo_CurrentDailydiaryNull_NewDailydiaryCreated()
    {
        // Arrange
        var customerId = "123";
        var customerHealth = new CustomerHealth { customer_id = 123, weight = 70, tdee = 2000 };
        _mockLocalSettingsService.Setup(s => s.ReadSettingAsync<string>("customer_id")).ReturnsAsync(customerId);
        _mockApiServicesClient.Setup(c => c.Get<CustomerHealth>(It.IsAny<string>())).Returns(customerHealth);
        _mockApiServicesClient.Setup(c => c.Get<Dailydiary>(It.IsAny<string>())).Returns((Dailydiary)null);
        _mockDateTimeProvider.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

        // Act
        await Task.Run(() => _viewModel.OnNavigatedTo(null));

        // Assert
        _mockApiServicesClient.Verify(c => c.Post<Dailydiary>("api/Dailydiary/create", It.IsAny<Dailydiary>()), Times.Once);
        Assert.AreEqual("70", _viewModel.WeightText);
        Assert.AreEqual("2000", _viewModel.GoalCalories);
        Assert.AreEqual("2000", _viewModel.RemainCalories);
        Assert.AreEqual("0", _viewModel.BurnedCalories);
        Assert.AreEqual(0, _viewModel.ProgressValue);
    }

    [TestMethod] // Test case kiểm tra báo lỗi khi gọi đến api để thêm daily diary thất bại
    public async Task OnNavigatedTo_ShouldShowErrorDialog_WhenApiPostFails()
    {
        // Arrange
        _mockApiServicesClient
            .Setup(client => client.Post<Dailydiary>(It.IsAny<string>(), It.IsAny<Dailydiary>()))
            .Throws(new Exception("API POST failed"));

        _mockLocalSettingsService
            .Setup(service => service.ReadSettingAsync<string>("customer_id"))
            .ReturnsAsync("123");

        _mockApiServicesClient
            .Setup(client => client.Get<CustomerHealth>(It.IsAny<string>()))
            .Returns(new CustomerHealth { customer_id = 123, weight = 70, tdee = 2000 });

        _mockDateTimeProvider
            .Setup(provider => provider.UtcNow)
            .Returns(DateTime.Now);

        // Act
        await Task.Run(() => _viewModel!.OnNavigatedTo(null));

        // Assert
        _mockDialogService.Verify(dialog => dialog.ShowErrorDialogAsync(It.Is<string>(s => s.Contains("API POST failed"))), Times.Once);
    }

    [TestMethod] // Test case kiểm tra có dữ liệu từ plandetail và là ngày tập 
    public async Task OnNavigatedTo_PlandetailsNotNull_CurrentDayWorkoutDetailFound()
    {
        // Arrange
        var currentDate = new DateTime(2024, 11, 25);
        var customerId = "123";
        var plandetail = new Plandetail
        {
            Workoutdetails = new List<Workoutdetail>
            {
                new Workoutdetail
                {
                    date_workout = currentDate,
                    Typeworkout = new Typeworkout { description = "Workout" },
                    description = "Đã hoàn thành Exercise trong ngày"
                }
            }
        };
        _mockLocalSettingsService.Setup(s => s.ReadSettingAsync<string>("customer_id")).ReturnsAsync(customerId);
        _mockApiServicesClient.Setup(c => c.Get<CustomerHealth>(It.IsAny<string>())).Returns(new CustomerHealth());
        _mockApiServicesClient.Setup(c => c.Get<Dailydiary>(It.IsAny<string>())).Returns(new Dailydiary());
        _mockApiServicesClient.Setup(c => c.Get<Plandetail>(It.IsAny<string>())).Returns(plandetail);
        _mockDateTimeProvider.Setup(d => d.Now).Returns(currentDate);

        // Act
        await Task.Run(() => _viewModel.OnNavigatedTo(null));

        // Assert
        Assert.AreEqual("Workout", _viewModel.TypeWorkoutDate);
        Assert.AreEqual(Visibility.Visible, _viewModel.StatusVisibility);
        Assert.AreEqual("Đã hoàn thành", _viewModel.ExerciseStatus);
        Assert.AreEqual("ms-appx:///Assets/Icon/ok.svg", _viewModel.StatusIconPath);
        Assert.AreEqual("ms-appx:///Assets/Icon/arm.svg", _viewModel.TypeWorkoutIconPath);
    }

    [TestMethod] // Test case kiểm tra có dữ liệu từ plandetail và là ngày nghỉ
    public async Task OnNavigatedTo_PlandetailsNotNull_CurrentDayWorkoutDetailNotFound()
    {
        // Arrange
        var customerId = "123";
        var plandetail = new Plandetail
        {
            Workoutdetails = new List<Workoutdetail>()
        };
        _mockLocalSettingsService.Setup(s => s.ReadSettingAsync<string>("customer_id")).ReturnsAsync(customerId);
        _mockApiServicesClient.Setup(c => c.Get<CustomerHealth>(It.IsAny<string>())).Returns(new CustomerHealth());
        _mockApiServicesClient.Setup(c => c.Get<Dailydiary>(It.IsAny<string>())).Returns(new Dailydiary());
        _mockApiServicesClient.Setup(c => c.Get<Plandetail>(It.IsAny<string>())).Returns(plandetail);
        _mockDateTimeProvider.Setup(d => d.Now).Returns(DateTime.Now);

        // Act
        await Task.Run(() => _viewModel.OnNavigatedTo(null));

        // Assert
        Assert.AreEqual("ms-appx:///Assets/Icon/bed.svg", _viewModel.TypeWorkoutIconPath);
        Assert.AreEqual("Ngày nghỉ", _viewModel.TypeWorkoutDate);
        Assert.AreEqual(Visibility.Collapsed, _viewModel.StatusVisibility);
    }

    [TestMethod] // Test case kiểm tra không có dữ liệu từ plandetail trả ra trường hợp chưa đăng ký kế hoạch
    public async Task OnNavigatedTo_PlandetailsNull()
    {
        // Arrange
        var customerId = "123";
        _mockLocalSettingsService.Setup(s => s.ReadSettingAsync<string>("customer_id")).ReturnsAsync(customerId);
        _mockApiServicesClient.Setup(c => c.Get<CustomerHealth>(It.IsAny<string>())).Returns(new CustomerHealth());
        _mockApiServicesClient.Setup(c => c.Get<Dailydiary>(It.IsAny<string>())).Returns(new Dailydiary());
        _mockApiServicesClient.Setup(c => c.Get<Plandetail>(It.IsAny<string>())).Returns((Plandetail)null);
        _mockDateTimeProvider.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

        // Act
        await Task.Run(() => _viewModel.OnNavigatedTo(null));

        // Assert
        Assert.AreEqual("ms-appx:///Assets/Icon/gymplan.svg", _viewModel.TypeWorkoutIconPath);
        Assert.AreEqual("Chưa có kế hoạch", _viewModel.TypeWorkoutDate);
        Assert.AreEqual(Visibility.Collapsed, _viewModel.StatusVisibility);
    }
}
