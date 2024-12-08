using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Gyminize.ViewModels;
using Gyminize.Contracts.Services;
using Gyminize.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Gyminize.Tests.MSTest.ViewModel;

/// <summary>
/// Lớp kiểm thử cho ViewModel của Nhật ký (DiaryViewModel).
/// </summary>
[TestClass]
public class DiaryViewModelTest
{
    private Mock<ILocalSettingsService> _mockLocalSettingsService;
    private Mock<IDateTimeProvider> _mockDateTimeProvider;
    private Mock<IApiServicesClient> _mockApiServicesClient;
    private Mock<IDialogService> _mockDialogService;
    private DiaryViewModel _viewModel;

    /// <summary>
    /// Thiết lập môi trường kiểm thử trước khi thực thi các phương thức kiểm thử.
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
        _mockLocalSettingsService = new Mock<ILocalSettingsService>();
        _mockDateTimeProvider = new Mock<IDateTimeProvider>();
        _mockApiServicesClient = new Mock<IApiServicesClient>();
        _mockDialogService = new Mock<IDialogService>();
        _viewModel = new DiaryViewModel(_mockLocalSettingsService.Object, _mockApiServicesClient.Object, _mockDateTimeProvider.Object, _mockDialogService.Object);
    }

    /// <summary>
    /// Kiểm tra xem phương thức <see cref="DiaryViewModel.LoadDailyDiary"/> có điền đúng thông tin vào các bộ sưu tập khi nhận dữ liệu hợp lệ.
    /// </summary>
    [TestMethod]
    public void LoadDailyDiary_WithValidData_ShouldPopulateCollections()
    {
        // Arrange: Thiết lập dữ liệu đầu vào
        var date = DateTime.Now;
        var dailyDiary = new Dailydiary
        {
            Fooddetails = new List<FoodDetail>
            {
                new FoodDetail { meal_type = 1 },
                new FoodDetail { meal_type = 2 },
                new FoodDetail { meal_type = 3 },
                new FoodDetail { meal_type = 4 }
            },
            daily_weight = 70,
            total_calories = 2000,
            calories_remain = 500
        };
        _mockApiServicesClient.Setup(m => m.Get<Dailydiary>(It.IsAny<string>())).Returns(dailyDiary);

        // Act: Thực thi phương thức kiểm tra
        _viewModel.LoadDailyDiary(date);

        // Assert: Kiểm tra kết quả đầu ra
        Assert.AreEqual(1, _viewModel.BreakfastItems.Count);
        Assert.AreEqual(1, _viewModel.LunchItems.Count);
        Assert.AreEqual(1, _viewModel.DinnerItems.Count);
        Assert.AreEqual(1, _viewModel.SnackItems.Count);
        Assert.AreEqual(70, _viewModel.WeightText);
        Assert.AreEqual(1500, _viewModel.BurnedCalories);
        Assert.AreEqual(2000, _viewModel.TotalCalories);
    }

    /// <summary>
    /// Kiểm tra trường hợp phương thức <see cref="DiaryViewModel.LoadFullData"/> gặp lỗi khi gọi API và hiển thị thông báo lỗi.
    /// </summary>
    [TestMethod]
    public async Task LoadFullData_ShouldShowErrorDialog_WhenLoadDailyDiaryFails()
    {
        // Arrange: Thiết lập lỗi trong API
        var daySelected = DateTime.Now;
        _mockApiServicesClient.Setup(client => client.Get<Dailydiary>(It.IsAny<string>()))
            .Throws(new Exception("Daily diary API error"));

        // Act: Thực thi phương thức kiểm tra
        await _viewModel.LoadFullData(daySelected);

        // Assert: Kiểm tra xem phương thức hiển thị thông báo lỗi được gọi đúng
        _mockDialogService.Verify(dialog => dialog.ShowErrorDialogAsync(It.Is<string>(s => s.Contains("Daily diary API error"))), Times.Once);
    }

    /// <summary>
    /// Kiểm tra xem phương thức <see cref="DiaryViewModel.LoadWorkoudetails"/> có điền đúng thông tin khi nhận dữ liệu kế hoạch tập luyện hợp lệ.
    /// </summary>
    [TestMethod]
    public void LoadWorkoudetails_WithValidPlanDetails_ShouldSetWorkoutDayProperties()
    {
        // Arrange: Thiết lập dữ liệu kế hoạch tập luyện hợp lệ
        var date = DateTime.Now;
        var planDetail = new Plandetail
        {
            Plan = new Plan { plan_name = "Test Plan" },
            Workoutdetails = new List<Workoutdetail>
            {
                new Workoutdetail { date_workout = date, Typeworkout = new Typeworkout { description = "Test Workout" }, description = "Đã hoàn thành Exercise trong ngày" }
            }
        };
        _mockApiServicesClient.Setup(m => m.Get<Plandetail>(It.IsAny<string>())).Returns(planDetail);

        // Act: Thực thi phương thức kiểm tra
        _viewModel.LoadWorkoudetails(date);

        // Assert: Kiểm tra các thuộc tính đã được thiết lập đúng
        Assert.AreEqual("Test Plan", _viewModel.PlanNameText);
        Assert.AreEqual("Test Workout", _viewModel.TypeWorkoutText);
        Assert.AreEqual(2, _viewModel._exerciseStatus);
    }

    /// <summary>
    /// Kiểm tra trường hợp không có kế hoạch tập luyện (dữ liệu trả về là null).
    /// </summary>
    [TestMethod]
    public void LoadWorkoudetails_NoPlanDetail_ShouldSetDefaultProperties()
    {
        // Arrange: Thiết lập dữ liệu không có kế hoạch tập luyện
        var date = DateTime.Now;
        _mockApiServicesClient.Setup(m => m.Get<Plandetail>(It.IsAny<string>())).Returns((Plandetail)null);

        // Act: Thực thi phương thức kiểm tra
        _viewModel.LoadWorkoudetails(date);

        // Assert: Kiểm tra các thuộc tính mặc định đã được thiết lập đúng
        Assert.AreEqual("Chưa có kế hoạch", _viewModel.PlanNameText);
        Assert.AreEqual("Chưa có ngày tập", _viewModel.TypeWorkoutText);
        Assert.AreEqual(0, _viewModel._exerciseStatus);
    }

    /// <summary>
    /// Kiểm tra trường hợp ngày được trả về là ngày nghỉ (mặc dù vẫn nằm trong kế hoạch tập luyện).
    /// </summary>
    [TestMethod]
    public void LoadWorkoudetails_RestDay_ShouldSetRestDayProperties()
    {
        // Arrange: Thiết lập dữ liệu ngày nghỉ
        var date = DateTime.Now;
        var planDetail = new Plandetail
        {
            Plan = new Plan { plan_name = "Test Plan" },
            start_date = date.AddDays(-1),
            end_date = date.AddDays(1),
            Workoutdetails = new List<Workoutdetail>()
        };
        _mockApiServicesClient.Setup(m => m.Get<Plandetail>(It.IsAny<string>())).Returns(planDetail);

        // Act: Thực thi phương thức kiểm tra
        _viewModel.LoadWorkoudetails(date);

        // Assert: Kiểm tra các thuộc tính của ngày nghỉ đã được thiết lập đúng
        Assert.AreEqual("Test Plan", _viewModel.PlanNameText);
        Assert.AreEqual("Ngày nghỉ", _viewModel.TypeWorkoutText);
        Assert.AreEqual(1, _viewModel._exerciseStatus);
    }
}
