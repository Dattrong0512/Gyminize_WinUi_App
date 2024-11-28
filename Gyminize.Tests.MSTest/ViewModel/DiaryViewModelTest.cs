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
[TestClass]
public class DiaryViewModelTest
{
    private Mock<ILocalSettingsService> _mockLocalSettingsService;
    private Mock<IDateTimeProvider> _mockDateTimeProvider;
    private Mock<IApiServicesClient> _mockApiServicesClient;
    private Mock<IDialogService> _mockDialogService;
    private DiaryViewModel _viewModel;

    [TestInitialize]
    public void Setup()
    {
        _mockLocalSettingsService = new Mock<ILocalSettingsService>();
        _mockDateTimeProvider = new Mock<IDateTimeProvider>();
        _mockApiServicesClient = new Mock<IApiServicesClient>();
        _mockDialogService = new Mock<IDialogService>();
        _viewModel = new DiaryViewModel(_mockLocalSettingsService.Object, _mockApiServicesClient.Object, _mockDateTimeProvider.Object, _mockDialogService.Object);
    }

    [TestMethod]// Test case kiểm tra dữ liệu nhật ký ăn uống từ daily diary là hợp lệ
    public void LoadDailyDiary_WithValidData_ShouldPopulateCollections()
    {
        // Arrange
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

        // Act
        _viewModel.LoadDailyDiary(date);

        // Assert
        Assert.AreEqual(1, _viewModel.BreakfastItems.Count);
        Assert.AreEqual(1, _viewModel.LunchItems.Count);
        Assert.AreEqual(1, _viewModel.DinnerItems.Count);
        Assert.AreEqual(1, _viewModel.SnackItems.Count);
        Assert.AreEqual(70, _viewModel.WeightText);
        Assert.AreEqual(1500, _viewModel.BurnedCalories);
        Assert.AreEqual(2000, _viewModel.TotalCalories);
    }


    [TestMethod] // Test case kiểm tra trường hợp nhật ký dinh dưỡng  gặp lỗi api
    public async Task LoadFullData_ShouldShowErrorDialog_WhenLoadDailyDiaryFails()
    {
        // Arrange
        var daySelected = DateTime.Now;
        _mockApiServicesClient.Setup(client => client.Get<Dailydiary>(It.IsAny<string>()))
            .Throws(new Exception("Daily diary API error"));

        // Act
        await _viewModel.LoadFullData(daySelected);

        // Assert
        _mockDialogService.Verify(dialog => dialog.ShowErrorDialogAsync(It.Is<string>(s => s.Contains("Daily diary API error"))), Times.Once);
    }

    [TestMethod] // Test case kiểm tra trường hợp trả về dữ liệu là ngày tập luyện
    public void LoadWorkoudetails_WithValidPlanDetails_ShouldSetWorkoutDayProperties()
    {
        // Arrange
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

        // Act
        _viewModel.LoadWorkoudetails(date);

        // Assert
        Assert.AreEqual("Test Plan", _viewModel.PlanNameText);
        Assert.AreEqual("Test Workout", _viewModel.TypeWorkoutText);
        Assert.AreEqual(2, _viewModel._exerciseStatus);
    }

    [TestMethod] // Test case kiểm tra trường hợp dữ liệu trả về chưa đăng kí kế hoạch (mặc định)
    public void LoadWorkoudetails_NoPlanDetail_ShouldSetDefaultProperties()
    {
        // Arrange
        var date = DateTime.Now;
        _mockApiServicesClient.Setup(m => m.Get<Plandetail>(It.IsAny<string>())).Returns((Plandetail)null);

        // Act
        _viewModel.LoadWorkoudetails(date);

        // Assert
        Assert.AreEqual("Chưa có kế hoạch", _viewModel.PlanNameText);
        Assert.AreEqual("Chưa có ngày tập", _viewModel.TypeWorkoutText);
        Assert.AreEqual(0, _viewModel._exerciseStatus);
    }

    [TestMethod] // Test case kiểm tra trường hợp dữ liệu trả về là ngày nghỉ (vẫn nằm trong kế hoạch)
    public void LoadWorkoudetails_RestDay_ShouldSetRestDayProperties()
    {
        // Arrange
        var date = DateTime.Now;
        var planDetail = new Plandetail
        {
            Plan = new Plan { plan_name = "Test Plan" },
            start_date = date.AddDays(-1),
            end_date = date.AddDays(1),
            Workoutdetails = new List<Workoutdetail>()
        };
        _mockApiServicesClient.Setup(m => m.Get<Plandetail>(It.IsAny<string>())).Returns(planDetail);

        // Act
        _viewModel.LoadWorkoudetails(date);

        // Assert
        Assert.AreEqual("Test Plan", _viewModel.PlanNameText);
        Assert.AreEqual("Ngày nghỉ", _viewModel.TypeWorkoutText);
        Assert.AreEqual(1, _viewModel._exerciseStatus);
    }
}
