using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gyminize.Contracts.Services;
using Gyminize.ViewModels;
using Gyminize.Models;
using Moq;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;

namespace Gyminize.Tests.MSTest.ViewModel;

/// <summary>
/// Lớp kiểm thử dành cho PlanViewModel/>.
/// </summary>
[TestClass]
public class PlanViewModelTest
{
    private Mock<ILocalSettingsService>? _mockLocalSettingsService;
    private Mock<IDialogService>? _mockDialogService;
    private Mock<INavigationService>? _mockNavigationService;
    private Mock<IApiServicesClient>? _mockApiServicesClient;
    private Mock<IDateTimeProvider>? _mockDateTimeProvider;
    private PlanViewModel? _viewModel;

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
        _mockDateTimeProvider = new Mock<IDateTimeProvider>();

        // Khởi tạo ViewModel với các mock được cung cấp
        _viewModel = new PlanViewModel(
            _mockNavigationService.Object,
            _mockDialogService.Object,
            _mockLocalSettingsService.Object,
            _mockApiServicesClient.Object,
            _mockDateTimeProvider.Object
        );
    }

    /// <summary>
    /// Kiểm tra khi tải thông tin kế hoạch thành công, các thuộc tính trong ViewModel được thiết lập chính xác.
    /// </summary>
    [TestMethod]
    public void LoadPlanDetailData_ShouldSetCorrectProperties_WhenApiReturnsValidData()
    {
        // Arrange
        var mockPlandetail = new Plandetail
        {
            start_date = new DateTime(2024, 11, 12),
            end_date = new DateTime(2025, 1, 7),
            Plan = new Plan { plan_name = "Kế Hoạch 4 Ngày" },
            Workoutdetails = new List<Workoutdetail>
        {
            new Workoutdetail { date_workout = new DateTime(2024, 11, 13), typeworkout_id = 1 },
            new Workoutdetail { date_workout = new DateTime(2024, 11, 14), typeworkout_id = 1 }
        }
        };

        _mockApiServicesClient
            .Setup(client => client.Get<Plandetail>(It.IsAny<string>()))
            .Returns(mockPlandetail);

        // Act
        _viewModel.LoadPlanDetailData();

        // Assert
        Assert.AreEqual(new DateTime(2024, 11, 12), _viewModel.StartDate);
        Assert.AreEqual(new DateTime(2025, 1, 7), _viewModel.EndDate);
        Assert.AreEqual("Kế Hoạch 4 Ngày", _viewModel.PlanName);
        Assert.AreEqual(2, _viewModel.WorkoutDetailsItems.Count);
    }

    /// <summary>
    /// Kiểm tra xử lý trường hợp danh sách ngày tập trong kế hoạch trống.
    /// </summary>
    [TestMethod] 
    public void LoadPlanDetailData_ShouldHandleEmptyWorkoutDetails()
    {
        // Arrange
        var mockPlandetail = new Plandetail
        {
            start_date = new DateTime(2024, 11, 12),
            end_date = new DateTime(2025, 1, 7),
            Workoutdetails = null // Danh sách trống
        };

        _mockApiServicesClient
            .Setup(client => client.Get<Plandetail>(It.IsAny<string>()))
            .Returns(mockPlandetail);

        _mockDialogService!
            .Setup(d => d.ShowErrorDialogAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        _viewModel.LoadPlanDetailData();

        // Assert
        Assert.AreEqual(new DateTime(2024, 11, 12), _viewModel.StartDate);
        Assert.AreEqual(new DateTime(2025, 1, 7), _viewModel.EndDate);
        _mockDialogService.Verify(d =>
            d.ShowErrorDialogAsync(It.Is<string>(msg => msg.Contains("Lỗi hệ thống"))),
            Times.Once);
    }


    /// <summary>
    /// Kiểm tra khi load thông tin kế hoạch thất bại do lỗi API, đảm bảo thông báo lỗi hiển thị đúng.
    /// </summary>
    [TestMethod] 
    public void LoadPlanDetailData_ShouldShowErrorDialog_WhenApiThrowsException()
    {
        // Arrange
        _mockApiServicesClient
            .Setup(client => client.Get<Plandetail>(It.IsAny<string>()))
            .Throws(new Exception("API Error"));

        _mockDialogService!
                .Setup(d => d.ShowErrorDialogAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

        // Act
        _viewModel.LoadPlanDetailData();

        // Assert
        _mockDialogService.Verify(service =>
            service.ShowErrorDialogAsync(It.Is<string>(s => s.Contains("API Error"))),
            Times.Once);
    }

    /// <summary>
    /// Kiểm tra logic tính toán thông tin ngày tập khi ngày hiện tại có bài tập và đã hoàn thành.
    /// </summary>
    [TestMethod] 
    public void LoadCurrentWeekDays_ShouldSetCorrectWeekNumberAndDaysHaveCompletedExercise()
    {
        // Arrange
        var startDate = new DateTime(2024, 11, 12); // Giả lập ngày bắt đầu kế hoạch
        var currentDate = new DateTime(2024, 11, 15); // Giả lập ngày hiện tại
        var workoutDetails = new List<Workoutdetail>
    {
        new Workoutdetail { date_workout = new DateTime(2024, 11, 15), description = "Đã hoàn thành Exercise trong ngày", typeworkout_id = 1 }
    };

        _mockDateTimeProvider.Setup(dt => dt.Now).Returns(currentDate);
        _viewModel.WorkoutDetailsItems = workoutDetails;

        // Act
        _viewModel.LoadCurrentWeekDays(startDate);

        // Assert
        Assert.AreEqual(1, _viewModel.WeekNumber); // Kiểm tra tuần thứ 1
        Assert.AreEqual("15/11", _viewModel.Day4); // Ngày thứ 4 của tuần hiện tại tính theo dữ liệu giả
        Assert.AreEqual("Bạn đã hoàn thành bài tập ngày hôm nay (15/11)", _viewModel.StatusText); // Trạng thái bài tập
        Assert.AreEqual("Tập lại", _viewModel.StartExerciseText); // Nút hành động
        Assert.AreEqual(Visibility.Visible, _viewModel.WorkoutButtonVisibility); // Nút hiển thị
    }

    /// <summary>
    /// Kiểm tra logic tính toán thông tin ngày tập khi ngày hiện tại có bài tập nhưng chưa hoàn thành.
    /// </summary>
    [TestMethod]
    public void LoadCurrentWeekDays_ShouldSetCorrectWeekNumberAndDaysHaveUncompletedExercise()
    {
        // Arrange
        var startDate = new DateTime(2024, 11, 12); // Giả lập ngày bắt đầu kế hoạch
        var currentDate = new DateTime(2024, 11, 23); // Giả lập ngày hiện tại
        var workoutDetails = new List<Workoutdetail>
    {
        new Workoutdetail { date_workout = new DateTime(2024, 11, 23), description = "", typeworkout_id = 2 }
    };

        _mockDateTimeProvider.Setup(dt => dt.Now).Returns(currentDate);
        _viewModel.WorkoutDetailsItems = workoutDetails;

        // Act
        _viewModel.LoadCurrentWeekDays(startDate);

        // Assert
        Assert.AreEqual(2, _viewModel.WeekNumber); // Kiểm tra tuần thứ 2
        Assert.AreEqual("23/11", _viewModel.Day5); // Ngày thứ 5 của tuần hiện tại tính theo dữ liệu giả
        Assert.AreEqual("Bạn vẫn chưa hoàn thành bài tập hôm nay (23/11)", _viewModel.StatusText); // Trạng thái bài tập
        Assert.AreEqual("Bắt đầu bài tập", _viewModel.StartExerciseText); // Nút hành động
        Assert.AreEqual(Visibility.Visible, _viewModel.WorkoutButtonVisibility); // Nút hiển thị
    }


    /// <summary>
    /// Kiểm tra logic tính toán thông tin ngày tập, trường hợp ngày không có bài tập (ngày nghỉ).
    /// </summary>
    [TestMethod] 
    public void LoadCurrentWeekDays_ShouldSetCorrectWeekNumberAndDaysHaveNoExercise()
    {
        // Arrange
        var startDate = new DateTime(2024, 11, 12); // Giả lập ngày bắt đầu kế hoạch
        var currentDate = new DateTime(2024, 12, 01); // Giả lập ngày hiện tại
        var workoutDetails = new List<Workoutdetail>
    {
        new Workoutdetail { date_workout = new DateTime(2024, 12, 01), description = "", typeworkout_id = 0 }
    };

        _mockDateTimeProvider.Setup(dt => dt.Now).Returns(currentDate);
        _viewModel.WorkoutDetailsItems = workoutDetails;

        // Act
        _viewModel.LoadCurrentWeekDays(startDate);

        // Assert
        Assert.AreEqual(3, _viewModel.WeekNumber); // Kiểm tra tuần thứ 3
        Assert.AreEqual("01/12", _viewModel.Day6); // Ngày thứ 6 của tuần hiện tại tính theo dữ liệu giả
        Assert.AreEqual("Hôm nay là ngày nghỉ", _viewModel.StatusText); // Trạng thái bài tập
        Assert.AreEqual(Visibility.Collapsed, _viewModel.WorkoutButtonVisibility); // Nút hiển thị
    }

    /// <summary>
    /// Kiểm tra trường hợp dữ liệu ngày tập bị trống (do trigger lỗi), đảm bảo tuần vẫn được khởi tạo.
    /// </summary>
    [TestMethod] 
    public void LoadCurrentWeekDays_ShouldHandleEmptyWorkoutDetails()
    {
        // Arrange
        var startDate = new DateTime(2024, 11, 12); // Giả lập ngày bắt đầu kế hoạch
        var currentDate = new DateTime(2024, 12, 01); // Giả lập ngày hiện tại
        var workoutDetails = new List<Workoutdetail>();

        _mockDateTimeProvider.Setup(dt => dt.Now).Returns(currentDate);
        _viewModel.WorkoutDetailsItems = workoutDetails;

        // Act
        _viewModel.LoadCurrentWeekDays(startDate);

        // Assert
        Assert.IsNotNull(_viewModel.WeekDaysItems); // Vẫn khởi tạo tuần với ngày mặc định
        Assert.AreEqual(7, _viewModel.WeekDaysItems.Count); // 7 ngày
    }

    /// <summary>
    /// Kiểm tra logic người dùng chọn thực hiện bài tập, xem hết và hoàn thành bài tập.
    /// </summary>
    [TestMethod] 
    public async Task PlayingWorkoutExercises_ShouldCallApiAndUpdateStatus_WhenExerciseIsFinished()
    {
        // Arrange
        var startDate = new DateTime(2024, 11, 12); // Giả lập ngày bắt đầu kế hoạch
        var currentDate = new DateTime(2024, 11, 23); // Giả lập ngày hiện tại

        _mockDateTimeProvider.Setup(dt => dt.Now).Returns(currentDate);
        _viewModel.WeekDaysItems = new ObservableCollection<Workoutdetail>
    {
        new Workoutdetail
        {
            date_workout = currentDate,
            description = "",
            typeworkout_id = 2,
            workoutdetail_id = 123,
            Typeworkout = new Typeworkout
            {
                Exercisedetails = new List<Exercisedetail>
                {
                    new Exercisedetail { Exercise = new Exercise { exercise_name = "Push-up" } }
                }
            }
        }
    };
        _mockDialogService
        .Setup(d => d.ShowFullExerciseWorkoutDialogAsync(It.IsAny<List<Exercisedetail>>()))
        .ReturnsAsync(true); // Người dùng hoàn thành bài tập

        // Act
        await _viewModel.PlayingWorkoutExercises();

        // Assert
        Assert.AreEqual("Bạn đã hoàn thành bài tập ngày hôm nay (23/11)", _viewModel.StatusText); // Trạng thái bài tập
        Assert.AreEqual("Tập lại", _viewModel.StartExerciseText); // Nút hành động
        _mockApiServicesClient.Verify(client =>
            client.Put<Workoutdetail>(It.Is<string>(url => url.Contains("api/Workoutdetail/update/123")), null),
            Times.Once); // Đảm bảo API được gọi
    }


    /// <summary>
    /// Kiểm tra trường hợp người dùng chọn thực hiện bài tập, nhưng hủy giữa chừng.
    /// </summary>
    [TestMethod] 
    public async Task PlayingWorkoutExercises_ShouldNotCallApi_WhenExerciseIsNotFinished()
    {
        // Arrange
        var currentDate = new DateTime(2024, 11, 23);
        _mockDateTimeProvider.Setup(dt => dt.Now).Returns(currentDate);

        _viewModel.WeekDaysItems = new ObservableCollection<Workoutdetail>
    {
        new Workoutdetail
        {
            date_workout = currentDate,
            description = "",
            typeworkout_id = 2,
            workoutdetail_id = 123,
            Typeworkout = new Typeworkout
            {
                Exercisedetails = new List<Exercisedetail>
                {
                    new Exercisedetail { Exercise = new Exercise { exercise_name = "Push-up" } }
                }
            }
        }
    };
        _mockDialogService
            .Setup(d => d.ShowFullExerciseWorkoutDialogAsync(It.IsAny<List<Exercisedetail>>()))
            .ReturnsAsync(false); // Người dùng hủy bài tập

        // Act
        await _viewModel.PlayingWorkoutExercises();

        // Assert (kiểm tra không gọi cập nhật trên api và thông tin status và text trên button không có giá trị để thay đổi )
        _mockApiServicesClient.Verify(client =>
            client.Put<Workoutdetail>(It.IsAny<string>(), null),
            Times.Never); // Không gọi API
        Assert.IsNull(_viewModel.StatusText, "StatusText là null"); // Kiểm tra trạng thái không thay đổi
        Assert.IsNull(_viewModel.StartExerciseText, "StartExerciseText là null"); // Kiểm tra text nút không thay đổi
    }

    /// <summary>
    /// Kiểm tra trường hợp người dùng hoàn thành bài tập nhưng gọi API cập nhật thất bại.
    /// </summary>
    [TestMethod] 
    public async Task PlayingWorkoutExercises_ShouldShowErrorDialog_WhenApiPutFails()
    {
        // Arrange
        var currentDate = new DateTime(2024, 11, 23);
        _mockDateTimeProvider.Setup(dt => dt.Now).Returns(currentDate);

        _viewModel.WeekDaysItems = new ObservableCollection<Workoutdetail>
    {
        new Workoutdetail
        {
            date_workout = currentDate,
            description = "",
            typeworkout_id = 2,
            workoutdetail_id = 123,
            Typeworkout = new Typeworkout
            {
                Exercisedetails = new List<Exercisedetail>
                {
                    new Exercisedetail { Exercise = new Exercise { exercise_name = "Push-up" } }
                }
            }
        }
    };
        _mockDialogService
            .Setup(d => d.ShowFullExerciseWorkoutDialogAsync(It.IsAny<List<Exercisedetail>>()))
            .ReturnsAsync(true); // Người dùng hoàn thành bài tập

        _mockApiServicesClient
            .Setup(client => client.Put<Workoutdetail>(It.IsAny<string>(), null))
            .Throws(new Exception("API error")); // Giả lập lỗi khi gọi API Put

        // Act
        await _viewModel.PlayingWorkoutExercises();

        // Assert(kiểm tra có gọi cập nhật 1 lần trên api kèm mã lỗi và thông tin status và text trên button không có giá trị để thay đổi )
        Assert.IsNull(_viewModel.StatusText, "StatusText là null"); // Kiểm tra status không thay đổi
        Assert.IsNull(_viewModel.StartExerciseText, "StartExerciseText là null"); // Kiểm tra text nút không thay đổi
        _mockDialogService.Verify(service =>
            service.ShowErrorDialogAsync(It.Is<string>(msg => msg.Contains("không thể cập nhật trạng thái bài tập"))),
            Times.Once); // Kiểm tra hiển thị lỗi
    }
}
