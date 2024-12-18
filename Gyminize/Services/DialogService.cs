using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using CommunityToolkit.WinUI;
using Gyminize.Contracts.Services;
using Gyminize.Helpers;
using Microsoft.Web.WebView2.Core;
using Gyminize;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Text;
using Gyminize.Models;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics;
using Gyminize.Services;
using Gyminize.Core.Services;
using Microsoft.UI.Xaml.Media.Imaging;
using ColorCode.Compilation.Languages;
using Microsoft.AspNetCore.Mvc;
using Gyminize.Converters;
using Microsoft.UI.Xaml.Data;
using Windows.UI.Text;
using Microsoft.UI.Xaml.Controls.Primitives;


public class DialogService : IDialogService
{
    /// <summary>
    /// Hiển thị hộp thoại lựa chọn bữa ăn và số lượng sản phẩm.
    /// </summary>
    /// <returns>
    /// Trả về một tuple gồm tên bữa ăn được chọn và số lượng được người dùng nhập.
    /// </returns>
    public async Task<(string? selectedMeal, int Quantity)> ShowMealSelectionDialogAsync()
    {
        var comboBox = new ComboBox
        {
            Items = { "Bữa Sáng", "Bữa Trưa", "Bữa Tối", "Bữa Xế" },
            SelectedIndex = 0,
            BorderThickness = new Thickness(1),
            BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.DarkSlateBlue),
            Margin = new Thickness(0, 0, 0, 10)
        };

        var numberBox = new NumberBox
        {
            Minimum = 1,
            Maximum = 20,
            BorderThickness = new Thickness(1),
            BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.DarkSlateBlue),
            Value = 1,
            Header = "Số Lượng",
            SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline
        };

        var stackPanel = new StackPanel();
        stackPanel.Children.Add(comboBox);
        stackPanel.Children.Add(numberBox);

        var mealDialog = new ContentDialog
        {
            Title = "Chọn Bữa Ăn và Số Lượng",
            Content = stackPanel,
            PrimaryButtonText = "Thêm",
            CloseButtonText = "Hủy"
        };
        
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            mealDialog.XamlRoot = rootElement.XamlRoot;
        }

        var primaryButtonStyle = new Style(typeof(Button));
        primaryButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Microsoft.UI.Colors.DarkSlateBlue)));
        primaryButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Microsoft.UI.Colors.White)));
        primaryButtonStyle.Setters.Add(new Setter(Button.FontSizeProperty, 16.0));
        primaryButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5)));
        primaryButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(10)));

        mealDialog.PrimaryButtonStyle = primaryButtonStyle;
        mealDialog.CloseButtonStyle = primaryButtonStyle;

        string? selectedMeal = null;
        var quantity = 1;

        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        await dispatcherQueue.EnqueueAsync(async () =>
        {
            var result = await mealDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                selectedMeal = comboBox.SelectedItem?.ToString();
                quantity = (int)numberBox.Value;
            }
        });

        return (selectedMeal, quantity);
    }
    private int _currentExerciseIndex;
    private List<Exercisedetail> _workouts = new List<Exercisedetail>();
    private TextBlock _exerciseNameTextBlock;
    private TextBlock _exerciseRepsTextBlock;
    private WebView2 _exerciseVideoWebView;
    private ContentDialog _workoutDialog;
    /// <summary>
    /// Hiển thị hộp thoại bài tập đầy đủ, cho phép người dùng duyệt qua các bài tập và xem video hướng dẫn.
    /// </summary>
    /// <param name="workouts">Danh sách các bài tập chi tiết cần hiển thị trong hộp thoại.</param>
    /// <returns>Trả về giá trị boolean, true nếu người dùng đã hoàn thành các bài tập, false nếu chưa.</returns>
    public async Task<bool> ShowFullExerciseWorkoutDialogAsync(List<Exercisedetail> workouts)
    {
        _workouts = workouts;
        _currentExerciseIndex = 0;
        bool isFinished = false;

        _workoutDialog = new ContentDialog
        {
            Title = "Danh sách phát bài tập",
            PrimaryButtonText = "Previous",
            SecondaryButtonText = "Next",
        };
        _workoutDialog.Resources["ContentDialogMaxWidth"] = 2000;
        _workoutDialog.Resources["ContentDialogMaxHeight"] = 1500;

        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            _workoutDialog.XamlRoot = rootElement.XamlRoot;
        }

        _exerciseNameTextBlock = new TextBlock { FontSize = 24, FontWeight = FontWeights.Bold };
        _exerciseRepsTextBlock = new TextBlock { FontSize = 18 };
        _exerciseVideoWebView = new WebView2 { Width = 900, Height = 320 };
        _exerciseVideoWebView.HorizontalAlignment = HorizontalAlignment.Stretch;
        _exerciseVideoWebView.VerticalAlignment = VerticalAlignment.Stretch;

        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        grid.Children.Add(_exerciseNameTextBlock);
        Grid.SetRow(_exerciseNameTextBlock, 0);

        grid.Children.Add(_exerciseRepsTextBlock);
        Grid.SetRow(_exerciseRepsTextBlock, 1);

        grid.Children.Add(_exerciseVideoWebView);
        Grid.SetRow(_exerciseVideoWebView, 2);

        var primaryButtonStyle = new Style(typeof(Button));
        primaryButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Microsoft.UI.Colors.DarkSlateBlue)));
        primaryButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Microsoft.UI.Colors.White)));
        primaryButtonStyle.Setters.Add(new Setter(Button.FontSizeProperty, 16.0));
        primaryButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5)));
        primaryButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(10)));

        _workoutDialog.Content = grid;

        _workoutDialog.PrimaryButtonStyle = primaryButtonStyle;
        _workoutDialog.SecondaryButtonStyle = primaryButtonStyle;

        UpdateExerciseDialog();

        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        await dispatcherQueue.EnqueueAsync(async () =>
        {
            ContentDialogResult result;
            do
            {
                result = await _workoutDialog.ShowAsync();
                if (result == ContentDialogResult.Secondary)
                {
                    if (_currentExerciseIndex < _workouts.Count - 1)
                    {
                        NextExercise();
                    }
                    else
                    {
                        _workoutDialog.Hide();
                        isFinished = true;
                    }
                }
                else if (result == ContentDialogResult.Primary)
                {
                    PreviousExercise();
                }
            } while (!isFinished);
        });
        return isFinished;
    }

    /// <summary>
    /// Cập nhật thông tin trong hộp thoại bài tập, bao gồm tên bài tập, số lần tập và video hướng dẫn.
    /// </summary>
    private void UpdateExerciseDialog()
    {
        var currentExercise = _workouts[_currentExerciseIndex];
        _exerciseNameTextBlock.Text = currentExercise.Exercise.exercise_name;
        _exerciseRepsTextBlock.Text = $"Reps: {currentExercise.Exercise.reps}";
        _exerciseVideoWebView.Source = new Uri(currentExercise.Exercise.linkvideo);

        _workoutDialog.SecondaryButtonText = _currentExerciseIndex == _workouts.Count - 1 ? "Finish" : "Next";
        _workoutDialog.IsPrimaryButtonEnabled = _currentExerciseIndex > 0;
    }

    /// <summary>
    /// Chuyển sang bài tập tiếp theo trong danh sách bài tập.
    /// </summary>
    private void NextExercise()
    {
        if (_currentExerciseIndex < _workouts.Count - 1)
        {
            _currentExerciseIndex++;
            UpdateExerciseDialog();
        }
    }

    /// <summary>
    /// Quay lại bài tập trước đó trong danh sách bài tập.
    /// </summary>
    private void PreviousExercise()
    {
        if (_currentExerciseIndex > 0)
        {
            _currentExerciseIndex--;
            UpdateExerciseDialog();
        }
    }


    /// <summary>
    /// Hiển thị hộp thoại bài tập với tên bài tập, số lần tập, và video hướng dẫn.
    /// </summary>
    /// <param name="exercise">Đối tượng bài tập chứa thông tin bài tập và video.</param>
    public async Task ShowExerciseVideoDialogAsync(Exercise exercise)
    {
        var exerciseNameTextBlock = new TextBlock { FontSize = 24, FontWeight = FontWeights.Bold, Text = exercise.exercise_name };
        var exerciseRepsTextBlock = new TextBlock { FontSize = 18, Text = $"Reps: {exercise.reps}" };
        var exerciseVideoWebView = new WebView2 { Width = 900, Height = 320, HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };

        var primaryButtonStyle = new Style(typeof(Button));
        primaryButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Microsoft.UI.Colors.DarkSlateBlue)));
        primaryButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Microsoft.UI.Colors.White)));
        primaryButtonStyle.Setters.Add(new Setter(Button.FontSizeProperty, 16.0));
        primaryButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5)));
        primaryButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(10)));

        exerciseVideoWebView.Source = new Uri(exercise.linkvideo);

        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        grid.Children.Add(exerciseNameTextBlock);
        Grid.SetRow(exerciseNameTextBlock, 0);

        grid.Children.Add(exerciseRepsTextBlock);
        Grid.SetRow(exerciseRepsTextBlock, 1);

        grid.Children.Add(exerciseVideoWebView);
        Grid.SetRow(exerciseVideoWebView, 2);
        exerciseVideoWebView.HorizontalAlignment = HorizontalAlignment.Stretch;
        exerciseVideoWebView.VerticalAlignment = VerticalAlignment.Stretch;

        var titleTextBlock = new TextBlock
        {
            Text = "Bài Tập",
            FontSize = 24,
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkSlateBlue),
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 0, 0, 10)
        };

        var exerciseDialog = new ContentDialog
        {
            Title = titleTextBlock,
            Content = grid,
            CloseButtonText = "OK"
        };

        exerciseDialog.CloseButtonStyle = primaryButtonStyle;
        exerciseDialog.Resources["ContentDialogMaxWidth"] = 2000;
        exerciseDialog.Resources["ContentDialogMaxHeight"] = 1500;

        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            exerciseDialog.XamlRoot = rootElement.XamlRoot;
        }

        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        await dispatcherQueue.EnqueueAsync(async () =>
        {
            await exerciseDialog.ShowAsync();
        });
    }


    /// <summary>
    /// Hiển thị hộp thoại xác nhận mã qua email và kiểm tra mã nhập vào.
    /// </summary>
    /// <param name="email">Địa chỉ email người dùng.</param>
    /// <param name="code">Mã xác nhận được gửi đến email của người dùng.</param>
    /// <returns>Trả về <c>true</c> nếu mã xác nhận đúng, <c>false</c> nếu không.</returns>
    public async Task<bool> ShowVerificationDialogAsync(string email, string code)
    {
        // Tạo Grid để sắp xếp nội dung
        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Tiêu đề
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Email
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Mô tả
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // TextBox
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Trạng thái
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Gửi lại mã

        // Tiêu đề
        var titleTextBlock = new TextBlock
        {
            Text = "Kiểm tra email của bạn",
            FontSize = 24,
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(0, 0, 0, 10)
        };

        // Email
        var emailTextBlock = new TextBlock
        {
            Text = email,
            FontSize = 18,
            FontWeight = FontWeights.SemiBold,
            Margin = new Thickness(0, 0, 0, 10),
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 128, 128, 128)) // Màu xám
        };

        // Mô tả
        var descriptionTextBlock = new TextBlock
        {
            Text = "Chúng tôi đã gửi một mã xác nhận đến email của bạn. Vui lòng kiểm tra hộp thư và nhập mã để tiếp tục.",
            FontSize = 16,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 0, 0, 20),
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)) // Màu đen
        };

        // Tạo TextBox để người dùng nhập mã xác thực
        var verificationTextBox = new TextBox
        {
            PlaceholderText = "Nhập mã xác thực",
            Margin = new Thickness(0, 0, 0, 10)
        };

        // Tạo TextBlock cho trạng thái
        var statusTextBlock = new TextBlock
        {
            Text = string.Empty, // Trạng thái ban đầu trống
            FontSize = 14,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)), // Màu đỏ
            Visibility = Visibility.Collapsed, // Ẩn ban đầu
            Margin = new Thickness(0, 0, 0, 10)
        };

        // Tạo TextBlock cho "Không nhận được? Gửi lại mã"
        var resendEmailTextBlock = new TextBlock
        {
            Text = "Không nhận được? Gửi lại mã",
            FontSize = 16,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204)), // Màu xanh tùy chỉnh
            TextDecorations = Windows.UI.Text.TextDecorations.Underline,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        // Thêm sự kiện Click cho TextBlock
        resendEmailTextBlock.Tapped += (sender, e) =>
        {
            Random random = new Random();
            code = random.Next(0, 10000).ToString("D4"); // Tạo mã xác thực random
            sendVerificationCode(email, code);
        };

        // Thêm các thành phần vào Grid
        grid.Children.Add(titleTextBlock);
        Grid.SetRow(titleTextBlock, 0);

        grid.Children.Add(emailTextBlock);
        Grid.SetRow(emailTextBlock, 1);

        grid.Children.Add(descriptionTextBlock);
        Grid.SetRow(descriptionTextBlock, 2);

        grid.Children.Add(verificationTextBox);
        Grid.SetRow(verificationTextBox, 3);

        grid.Children.Add(statusTextBlock);
        Grid.SetRow(statusTextBlock, 4);

        grid.Children.Add(resendEmailTextBlock);
        Grid.SetRow(resendEmailTextBlock, 5);

        // Định kiểu cho nút xác nhận
        var primaryButtonStyle = new Style(typeof(Button));
        primaryButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204)))); // Nền xanh (RGB: 0, 102, 204)
        primaryButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)))); // Chữ trắng
        primaryButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0))); // Không viền
        primaryButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(5))); // Bo góc
        primaryButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5))); // Căn chỉnh Padding

        // Định kiểu cho nút đóng
        var closeButtonStyle = new Style(typeof(Button));
        closeButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204)))); // Nền xanh (RGB: 0, 102, 204)
        closeButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)))); // Chữ trắng
        closeButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0))); // Không viền
        closeButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(5))); // Bo góc
        closeButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5))); // Căn chỉnh Padding

        // Tạo ContentDialog với các nút Primary và Close
        var verificationDialog = new ContentDialog
        {
            Title = null, // Đặt tiêu đề trong nội dung chính thay vì thuộc tính Title
            Content = grid,
            PrimaryButtonText = "Xác nhận",
            CloseButtonText = "Hủy",
            PrimaryButtonStyle = primaryButtonStyle,
            CloseButtonStyle = closeButtonStyle
        };

        // Ngăn ContentDialog tự động đóng khi nhấn "Xác nhận"
        verificationDialog.Closing += (sender, args) =>
        {
            // Chỉ xử lý khi người dùng nhấn nút "Xác nhận"
            if (verificationDialog.Content != null && args.Result == ContentDialogResult.Primary)
            {
                string enteredCode = verificationTextBox.Text;

                // Nếu mã sai, hiển thị trạng thái và ngăn đóng dialog
                if (enteredCode != code)
                {
                    statusTextBlock.Text = "Mã xác thực không hợp lệ. Vui lòng thử lại.";
                    statusTextBlock.Visibility = Visibility.Visible;
                    args.Cancel = true; // Giữ dialog mở
                }
                else
                {
                    statusTextBlock.Visibility = Visibility.Collapsed; // Ẩn thông báo nếu mã đúng
                }
            }
        };

        // Đảm bảo rằng dialog được hiển thị trên UI thread
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            verificationDialog.XamlRoot = rootElement.XamlRoot;
        }

        var isValid = false;

        // Hiển thị dialog và chờ kết quả
        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        await dispatcherQueue.EnqueueAsync(async () =>
        {
            var result = await verificationDialog.ShowAsync();
            if (result == ContentDialogResult.Primary && statusTextBlock.Visibility == Visibility.Collapsed)
            {
                isValid = true; // Mã đúng
            }
        });
        return isValid;
    }

    /// <summary>
    /// Gửi mã xác thực đến email người dùng.
    /// </summary>
    /// <param name="email">Địa chỉ email người nhận.</param>
    /// <param name="code">Mã xác thực cần gửi đến người dùng.</param>
    public async void sendVerificationCode(string email, string code)
    {
        IEmailSender emailSender = new EmailSender();
        var subject = "Mã xác thực cho Gyminize App";
        var body = $"<h1>Mã xác thực của bạn là: {code}</h1>" +
                   $"<h1>Vui lòng không chia sẻ cho bất kì ai khác</h1>"; // HTML

        try
        {
            await emailSender.SendEmailAsync(email, subject, body);
            Console.WriteLine("Email sent successfully!"); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }
    }

    /// <summary>
    /// Hiển thị một hộp thoại yêu cầu người dùng nhập tên tài khoản.
    /// </summary>
    /// <returns>Trả về một tuple chứa email và tên tài khoản hợp lệ của người dùng nếu xác nhận thành công, ngược lại là chuỗi trống.</returns>
    public async Task<(string email, string username)> ShowUsernameInputDialog()
    {
        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Title
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // TextBox
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Status

        // Tạo TextBlock cho tiêu đề
        var titleTextBlock = new TextBlock
        {
            Text = "Nhập tên tài khoản",
            FontSize = 24,
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(0, 0, 0, 10)
        };

        // Tạo TextBox cho người dùng nhập tên tài khoản
        var usernameTextBox = new TextBox
        {
            PlaceholderText = "Tên đăng nhập",
            Margin = new Thickness(0, 0, 0, 10)
        };

        // Tạo TextBlock để hiển thị trạng thái lỗi nếu có
        var statusTextBlock = new TextBlock
        {
            Text = string.Empty, // Trạng thái ban đầu trống
            FontSize = 14,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)), // Màu đỏ
            Visibility = Visibility.Collapsed, // Ẩn ban đầu
            Margin = new Thickness(0, 0, 0, 10)
        };

        // Thêm các thành phần vào Grid
        grid.Children.Add(titleTextBlock);
        Grid.SetRow(titleTextBlock, 0);

        grid.Children.Add(usernameTextBox);
        Grid.SetRow(usernameTextBox, 1);

        grid.Children.Add(statusTextBlock);
        Grid.SetRow(statusTextBlock, 2);

        // Định kiểu cho các nút trong ContentDialog
        var primaryButtonStyle = new Style(typeof(Button));
        primaryButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204)))); // Nền xanh
        primaryButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)))); // Chữ trắng
        primaryButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0))); // Không viền
        primaryButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(5))); // Bo góc
        primaryButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5))); // Căn chỉnh Padding

        var closeButtonStyle = new Style(typeof(Button));
        closeButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204)))); // Nền xanh
        closeButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)))); // Chữ trắng
        closeButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0))); // Không viền
        closeButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(5))); // Bo góc
        closeButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5))); // Căn chỉnh Padding

        // Tạo ContentDialog cho việc nhập tên tài khoản
        var inputUsernameDialog = new ContentDialog
        {
            Title = null,
            Content = grid,
            PrimaryButtonText = "Xác nhận",
            CloseButtonText = "Hủy",
            PrimaryButtonStyle = primaryButtonStyle,
            CloseButtonStyle = closeButtonStyle
        };

        var validEmail = "";
        var validUsername = "";

        inputUsernameDialog.Closing += (sender, args) =>
        {
            if (inputUsernameDialog.Content != null && args.Result == ContentDialogResult.Primary)
            {
                string enteredUsername = usernameTextBox.Text;

                if (string.IsNullOrWhiteSpace(enteredUsername))
                {
                    statusTextBlock.Text = "Tên đăng nhập không được để trống";
                    statusTextBlock.Visibility = Visibility.Visible;
                    args.Cancel = true; 
                }
                else
                {
                    var endpoint = $"api/Customer/get/username/" + enteredUsername;
                    var _customerInfo = ApiServices.Get<Customer>(endpoint);
                    if (_customerInfo != null)
                    {
                        validEmail = _customerInfo.email; 
                        validUsername = enteredUsername;
                        statusTextBlock.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        statusTextBlock.Text = "Tên đăng nhập không tồn tại";
                        statusTextBlock.Visibility = Visibility.Visible;
                        args.Cancel = true; 
                    }
                }
            }
        };

        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            inputUsernameDialog.XamlRoot = rootElement.XamlRoot;
        }

        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        await dispatcherQueue.EnqueueAsync(async () =>
        {
            await inputUsernameDialog.ShowAsync();
        });

        return (validEmail, validUsername);
    }


    /// <summary>
    /// Hiển thị hộp thoại yêu cầu người dùng nhập mật khẩu mới và xác nhận lại mật khẩu.
    /// </summary>
    /// <returns>Trả về mật khẩu hợp lệ nếu người dùng nhập đúng, nếu không trả về chuỗi rỗng.</returns>
    public async Task<string> ShowResetPasswordDialogAsync()
    {
        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Title
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // PasswordBox
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // PBoxStatus
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // ConfirmPasswordBox
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // CPBoxStatus

        var titleTextBlock = new TextBlock
        {
            Text = "Đổi mật khẩu",
            FontSize = 24,
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(0, 0, 0, 10)
        };

        var passwordBox = new PasswordBox
        {
            PlaceholderText = "Mật khẩu mới",
            Margin = new Thickness(0, 0, 0, 10)
        };

        var passwordStatusTextBlock = new TextBlock
        {
            Text = string.Empty,
            FontSize = 14,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)),
            Visibility = Visibility.Collapsed,
            Margin = new Thickness(0, 0, 0, 10)
        };

        var confirmPasswordBox = new PasswordBox
        {
            PlaceholderText = "Nhập lại mật khẩu mới",
            Margin = new Thickness(0, 0, 0, 10)
        };

        var confirmPasswordStatusTextBlock = new TextBlock
        {
            Text = string.Empty,
            FontSize = 14,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)),
            Visibility = Visibility.Collapsed,
            Margin = new Thickness(0, 0, 0, 10)
        };

        grid.Children.Add(titleTextBlock);
        Grid.SetRow(titleTextBlock, 0);

        grid.Children.Add(passwordBox);
        Grid.SetRow(passwordBox, 1);

        grid.Children.Add(passwordStatusTextBlock);
        Grid.SetRow(passwordStatusTextBlock, 2);

        grid.Children.Add(confirmPasswordBox);
        Grid.SetRow(confirmPasswordBox, 3);

        grid.Children.Add(confirmPasswordStatusTextBlock);
        Grid.SetRow(confirmPasswordStatusTextBlock, 4);

        var primaryButtonStyle = new Style(typeof(Button));
        primaryButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204))));
        primaryButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255))));
        primaryButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0)));
        primaryButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(5)));
        primaryButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5)));

        var closeButtonStyle = new Style(typeof(Button));
        closeButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204))));
        closeButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255))));
        closeButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0)));
        closeButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(5)));
        closeButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5)));

        var resetPasswordDialog = new ContentDialog
        {
            Title = null,
            Content = grid,
            PrimaryButtonText = "Xác nhận",
            CloseButtonText = "Hủy",
            PrimaryButtonStyle = primaryButtonStyle,
            CloseButtonStyle = closeButtonStyle
        };

        var validPassword = "";

        resetPasswordDialog.Closing += (sender, args) =>
        {
            if (resetPasswordDialog.Content != null && args.Result == ContentDialogResult.Primary)
            {
                string enteredPassword = passwordBox.Password;
                string enteredConfirmPassword = confirmPasswordBox.Password;

                if (string.IsNullOrWhiteSpace(enteredPassword))
                {
                    passwordStatusTextBlock.Text = "Mật khẩu không được để trống";
                    passwordStatusTextBlock.Visibility = Visibility.Visible;
                    args.Cancel = true;
                }
                else if (string.IsNullOrWhiteSpace(enteredConfirmPassword))
                {
                    confirmPasswordStatusTextBlock.Text = "Mật khẩu nhập lại không được để trống";
                    confirmPasswordStatusTextBlock.Visibility = Visibility.Visible;
                    args.Cancel = true;
                }
                else if (enteredPassword != enteredConfirmPassword)
                {
                    confirmPasswordStatusTextBlock.Text = "Mật khẩu nhập lại không khớp";
                    confirmPasswordStatusTextBlock.Visibility = Visibility.Visible;
                    args.Cancel = true;
                }
                else
                {
                    validPassword = enteredPassword;
                    passwordStatusTextBlock.Visibility = Visibility.Collapsed;
                    confirmPasswordStatusTextBlock.Visibility = Visibility.Collapsed;
                }
            }
        };

        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            resetPasswordDialog.XamlRoot = rootElement.XamlRoot;
        }
        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        await dispatcherQueue.EnqueueAsync(async () =>
        {
            await resetPasswordDialog.ShowAsync();
        });
        return validPassword;
    }

    /// <summary>
    /// Hiển thị hộp thoại lỗi với thông báo lỗi và biểu tượng.
    /// </summary>
    /// <param name="errorMessage">Thông báo lỗi cần hiển thị trong hộp thoại.</param>
    public async Task ShowErrorDialogAsync(string errorMessage)
    {
        var errorIcon = new FontIcon
        {
            Glyph = "\uEB90", // Mã Unicode của biểu tượng lỗi
            FontSize = 50,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)), // Màu đỏ
            Margin = new Thickness(10, 10, 10, 10)
        };

        var errorTextBlock = new TextBlock
        {
            Text = errorMessage,
            FontSize = 16,
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Center,
            Margin = new Thickness(10, 10, 10, 10)
        };

        var stackPanel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Orientation = Orientation.Horizontal
        };
        stackPanel.Children.Add(errorIcon);
        stackPanel.Children.Add(errorTextBlock);

        var errorDialog = new ContentDialog
        {
            Title = "Error",
            Content = stackPanel,
            CloseButtonText = "OK"
        };

        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            errorDialog.XamlRoot = rootElement.XamlRoot;
        }
        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        await dispatcherQueue.EnqueueAsync(async () =>
        {
            await errorDialog.ShowAsync();
        });
    }

    /// <summary>
    /// Hiển thị hộp thoại thông tin sản phẩm, bao gồm hình ảnh, tên, giá, nhà cung cấp, số lượng và mô tả.
    /// Cho phép người dùng chọn số lượng và thêm sản phẩm vào giỏ hàng.
    /// </summary>
    /// <param name="product">Sản phẩm cần hiển thị thông tin.</param>
    /// <param name="orderid">ID đơn hàng hiện tại.</param>
    /// <returns>Trả về giá trị tương ứng với kết quả thêm sản phẩm vào giỏ hàng (1: thành công, 0: thất bại, 2: chưa thêm).</returns>
    public async Task<int> ShowProductDialogWithSupplierAsync(Product product, int orderid)
    {
        int addSuccess = 2;

        // Hình ảnh sản phẩm
        var productImage = new Image
        {
            Source = new BitmapImage(new Uri(product.product_source)),
            Width = 300,
            Height = 300,
            HorizontalAlignment = HorizontalAlignment.Left,
            Stretch = Stretch.Uniform,
            Margin = new Thickness(10)
        };

        // Tên sản phẩm
        var productNameTextBlock = new TextBlock
        {
            Text = product.product_name,
            FontSize = 20,
            FontWeight = FontWeights.Bold,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(10, 10, 10, 10)
        };

        // Giá sản phẩm
        var productPriceTextBlock = new TextBlock
        {
            FontSize = 18,
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red),
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(10, 10, 10, 10)
        };
        productPriceTextBlock.SetBinding(TextBlock.TextProperty, new Binding
        {
            Source = product.product_price,
            Converter = new PriceConverter()
        });

        // Nhà cung cấp
        var supplierTextBlock = new TextBlock
        {
            Text = product.product_provider,
            FontSize = 16,
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkGray),
            Margin = new Thickness(10, 10, 10, 10)
        };

        // Số lượng sản phẩm với NumberBox
        var quantityLabel = new TextBlock
        {
            Text = "Số lượng:",
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkSlateBlue),
            FontSize = 16,
            Margin = new Thickness(10, 20, 10, 10)
        };

        var quantityNumberBox = new NumberBox
        {
            BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.DarkSlateBlue),
            BorderThickness = new Thickness(1),
            Minimum = 1,
            Maximum = 20,
            Value = 1,
            Width = 150,
            HorizontalAlignment = HorizontalAlignment.Left,
            SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline,
            Margin = new Thickness(10, 10, 10, 10)
        };

        var quantityPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(10, 10, 10, 10)
        };
        quantityPanel.Children.Add(quantityLabel);
        quantityPanel.Children.Add(quantityNumberBox);

        var descriptionLabel = new TextBlock
        {
            Text = "Mô tả:",
            FontSize = 16,
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkSlateBlue),
            Margin = new Thickness(10, 10, 10, 10)
        };

        var descriptionTextBlock = new TextBlock
        {
            Text = product.description,
            FontSize = 13,
            FontStyle = FontStyle.Italic,
            Width = 500,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(10, 10, 10, 10)
        };

        // Lưới bố cục chính
        var mainGrid = new Grid();
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });

        // Hình ảnh
        mainGrid.Children.Add(productImage);
        Grid.SetColumn(productImage, 0);

        // Nội dung bên phải
        var rightPanel = new StackPanel
        {
            Orientation = Orientation.Vertical
        };

        rightPanel.Children.Add(productNameTextBlock);
        rightPanel.Children.Add(productPriceTextBlock);
        rightPanel.Children.Add(supplierTextBlock); // Thêm mục Nhà cung cấp
        rightPanel.Children.Add(quantityPanel);
        rightPanel.Children.Add(descriptionLabel);
        rightPanel.Children.Add(descriptionTextBlock);

        mainGrid.Children.Add(rightPanel);
        Grid.SetColumn(rightPanel, 1);

        // Tạo dialog
        var productDialog = new ContentDialog
        {
            Title = "Thông tin sản phẩm",
            Content = mainGrid,
            PrimaryButtonText = "Thêm vào giỏ hàng",
            CloseButtonText = "Hủy",
        };

        var primaryButtonStyle = new Style(typeof(Button));
        primaryButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Microsoft.UI.Colors.DarkSlateBlue)));
        primaryButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Microsoft.UI.Colors.White)));
        primaryButtonStyle.Setters.Add(new Setter(Button.FontSizeProperty, 16.0));
        primaryButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5)));
        primaryButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(10)));

        productDialog.PrimaryButtonStyle = primaryButtonStyle;
        productDialog.CloseButtonStyle = primaryButtonStyle;

        productDialog.Resources["ContentDialogMaxWidth"] = 1200;
        productDialog.Resources["ContentDialogMaxHeight"] = 600;

        // Xử lý khi người dùng nhấn "Thêm vào giỏ hàng"
        productDialog.PrimaryButtonClick += (s, e) =>
        {
            var orderdetail = new Orderdetail
            {
                product_id = product.product_id,
                Product = product,
                product_amount = (int)quantityNumberBox.Value,
                orders_id = orderid
            };
            var postResult = ApiServices.Post<Orderdetail>("api/OrderDetail/add", orderdetail);
            if (postResult == null)
            {
                addSuccess = 0; // Không thành công
            }
            else
            {
                addSuccess = 1; // Thành công
            }
        };

        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            productDialog.XamlRoot = rootElement.XamlRoot;
        }

        await productDialog.ShowAsync();
        return addSuccess;
    }

    /// <summary>
    /// Hiển thị thông báo thành công trong một cửa sổ pop-up ở giữa màn hình.
    /// Thông báo sẽ được hiển thị trong 2 giây và tự động đóng lại.
    /// </summary>
    /// <param name="message">Thông điệp thành công cần hiển thị.</param>
    public async Task ShowSuccessMessageAsync(string message)
    {
        // Tạo TextBlock hiển thị thông báo
        var textBlock = new TextBlock
        {
            Text = message,
            Padding = new Thickness(20),
            FontSize = 16,
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkGreen)
        };

        // Tạo Border bao quanh TextBlock
        var border = new Border
        {
            Background = new SolidColorBrush(Microsoft.UI.Colors.PaleGreen), // Nền màu xanh nhạt
            Child = textBlock,
            CornerRadius = new CornerRadius(10), // Bo tròn góc của border
            Margin = new Thickness(0, 20, 0, 0)
        };

        // Tạo Popup để hiển thị thông báo
        var popup = new Popup
        {
            Child = border,
            IsLightDismissEnabled = true // Cho phép đóng popup khi người dùng nhấn ra ngoài
        };

        // Gán XamlRoot của popup cho cửa sổ chính
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            popup.XamlRoot = rootElement.XamlRoot;
        }

        // Thực hiện hiển thị popup với các offset tùy chỉnh
        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        await dispatcherQueue.EnqueueAsync(() =>
        {
            var windowBounds = App.MainWindow.Bounds;
            var popupWidth = border.ActualWidth;
            var popupHeight = border.ActualHeight;

            // Đặt vị trí của popup để hiển thị ở giữa màn hình
            popup.HorizontalOffset = ((windowBounds.Width - popupWidth) / 2) - 80;
            popup.VerticalOffset = 20;
            popup.IsOpen = true; // Mở popup
        });

        // Đợi 2 giây rồi đóng popup
        await Task.Delay(2000);

        // Đóng popup
        popup.IsOpen = false;
    }

    private const int MaxPollingTime = 9000000; // Tối đa  15p

    public class ApiResponse
    {
        public string message
        {
            get; set;
        }
        public string Status
        {
            get; set;
        } // Optional: cho các trường hợp có status
        public int OrderId
        {
            get; set;
        }   // Optional: cho các trường hợp có orderId
        public decimal PaymentAmount
        {
            get; set;
        } // Optional: nếu có paymentAmount
    }

    public async Task<int> ShowVNPAYPaymentProcessDialogAsync(int orderId)
    {
        int status = 0;
        Debug.WriteLine("Bắt đầu ShowVNPAYPaymentProcessDialogAsync...");

        var progressRing = new ProgressRing
        {
            Visibility = Visibility.Visible,
            IsActive = true,
            Width = 50,
            Height = 50,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var statusIcon = new FontIcon
        {
            Visibility = Visibility.Collapsed,
            Glyph = "\uEA3A", // Default icon
            FontSize = 50,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var textStatus = new TextBlock
        {
            Text = "Đơn hàng đang thanh toán",
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 0, 0, 20)
        };

        var dialog = new ContentDialog
        {
            Content = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Children =
            {
                textStatus,
                progressRing,
                statusIcon
            }
            },
            CloseButtonText = "Hủy",
            DefaultButton = ContentDialogButton.Close
        };

        Debug.WriteLine("ContentDialog đã được khởi tạo.");

        // Set dialog root element
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            dialog.XamlRoot = rootElement.XamlRoot;
            Debug.WriteLine("XamlRoot đã được thiết lập.");
        }

        // Hiển thị dialog trước khi chạy polling
        var showDialogTask = dialog.ShowAsync().AsTask();
        Debug.WriteLine("ContentDialog đang được hiển thị...");

        // Chạy polling trong Task ngầm
        var pollingTask = Task.Run(async () =>
        {
            var endpoint = $"api/Cart/check-payment-status/{orderId}";
            int elapsedTime = 0;

            Debug.WriteLine($"Bắt đầu polling API với endpoint: {endpoint}");

            while (elapsedTime < MaxPollingTime)
            {
                try
                {
                    Debug.WriteLine($"Gọi API lần {elapsedTime / 5000 + 1}...");
                    var orderStatus = ApiServices.Get<ApiResponse>(endpoint);
                    Debug.WriteLine($"API trả về: {orderStatus?.message}");

                    // Nếu thanh toán thành công
                    if (orderStatus.message == "Đơn hàng thanh toán thành công")
                    {
                        Debug.WriteLine("Trạng thái: Thành công.");
                        status = 1;

                        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
                        if (dispatcherQueue != null)
                        {
                            await dispatcherQueue.EnqueueAsync(() =>
                            {
                                progressRing.Visibility = Visibility.Collapsed;
                                statusIcon.Glyph = "\uE930"; // Success icon
                                statusIcon.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                                statusIcon.Visibility = Visibility.Visible;
                                textStatus.Text = "Đơn hàng thanh toán thành công";
                                dialog.CloseButtonText = "OK";
                            });
                        }
                        else
                        {
                            await App.MainWindow.DispatcherQueue.EnqueueAsync(() =>
                            {
                                progressRing.Visibility = Visibility.Collapsed;
                                statusIcon.Glyph = "\uE930"; // Success icon
                                statusIcon.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                                statusIcon.Visibility = Visibility.Visible;
                                textStatus.Text = "Đơn hàng thanh toán thành công";
                                dialog.CloseButtonText = "OK";
                            });
                            Debug.WriteLine("Trạng thái: chỉnh thành công(main win).");
                        }
                        Debug.WriteLine("Trạng thái: chỉnh thành công.");
                        break;
                    }
                    // Nếu thanh toán thất bại
                    else if (orderStatus.message == "Đơn hàng thanh toán thất bại")
                    {
                        Debug.WriteLine("Trạng thái: Thất bại.");
                        status = -1;

                        await DispatcherQueue.GetForCurrentThread().EnqueueAsync(() =>
                        {
                            progressRing.Visibility = Visibility.Collapsed;
                            statusIcon.Glyph = "\uE8D7"; // Failure icon
                            statusIcon.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                            statusIcon.Visibility = Visibility.Visible;
                            textStatus.Text = "Đơn hàng thanh toán thất bại";
                            dialog.CloseButtonText = "OK";
                        });
                        break;
                    }

                    // Tạm dừng 5 giây trước khi polling lại
                    await Task.Delay(5000);
                    elapsedTime += 5000;
                    Debug.WriteLine($"Đã chờ {elapsedTime} ms, tiếp tục polling...");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Lỗi khi gọi API: {ex.Message}");

                    await DispatcherQueue.GetForCurrentThread().EnqueueAsync(() =>
                    {
                        progressRing.Visibility = Visibility.Collapsed;
                        statusIcon.Glyph = "\uE8C9"; // Error icon
                        statusIcon.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Gray);
                        statusIcon.Visibility = Visibility.Visible;
                        textStatus.Text = "Lỗi kết nối";
                        dialog.CloseButtonText = "Lỗi";
                    });
                    break;
                }
            }

            if (elapsedTime >= MaxPollingTime)
            {
                Debug.WriteLine("Đã hết thời gian chờ polling.");
                await DispatcherQueue.GetForCurrentThread().EnqueueAsync(() =>
                {
                    progressRing.Visibility = Visibility.Collapsed;
                    statusIcon.Glyph = "\uE8C9"; // Timeout icon
                    statusIcon.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Gray);
                    statusIcon.Visibility = Visibility.Visible;
                    textStatus.Text = "Hết thời gian chờ";
                    dialog.CloseButtonText = "Timeout";
                });
            }
        });

        // Chờ polling hoàn thành
        await pollingTask;

        // Chờ người dùng đóng dialog
        await showDialogTask;

        Debug.WriteLine("Kết thúc ShowVNPAYPaymentProcessDialogAsync.");
        return status;
    }
}