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


public class DialogService : IDialogService
{
    public async Task<(string? selectedMeal, int Quantity)> ShowMealSelectionDialogAsync()
    {
        // Tạo ComboBox để chọn bữa ăn
        var comboBox = new ComboBox
        {
            Items = { "Bữa Sáng", "Bữa Trưa", "Bữa Tối", "Bữa Xế" },
            SelectedIndex = 0,
            Margin = new Thickness(0, 0, 0, 10)
        };

        // Tạo NumberBox để nhập số lượng
        var numberBox = new NumberBox
        {
            Minimum = 1,
            Maximum = 20,
            Value = 1,
            Header = "Số Lượng",
            SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline
        };

        // Tạo StackPanel để chứa ComboBox và NumberBox
        var stackPanel = new StackPanel();
        stackPanel.Children.Add(comboBox);
        stackPanel.Children.Add(numberBox);

        // Tạo ContentDialog với StackPanel là nội dung
        var mealDialog = new ContentDialog
        {
            Title = "Chọn Bữa Ăn và Số Lượng",
            Content = stackPanel,
            PrimaryButtonText = "Thêm",
            CloseButtonText = "Hủy"
        };

        // Thiết lập XamlRoot nếu dùng WinUI 3
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            mealDialog.XamlRoot = rootElement.XamlRoot;
        }

        string? selectedMeal = null;
        var quantity = 1;

        // Đảm bảo gọi trên UI Thread
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
        // Thiết lập XamlRoot nếu dùng WinUI 3
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            _workoutDialog.XamlRoot = rootElement.XamlRoot;
        }

        // Tạo UI cho bài tập hiện tại
        _exerciseNameTextBlock = new TextBlock { FontSize = 24, FontWeight = FontWeights.Bold };
        _exerciseRepsTextBlock = new TextBlock { FontSize = 18 };
        _exerciseVideoWebView = new WebView2 { Width = 900, Height = 320 };
        _exerciseVideoWebView.HorizontalAlignment = HorizontalAlignment.Stretch;
        _exerciseVideoWebView.VerticalAlignment = VerticalAlignment.Stretch;


        // Sắp xếp các thành phần trong StackPanel
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


        _workoutDialog.Content = grid;

        UpdateExerciseDialog();

        // Đảm bảo gọi trên UI Thread
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

    private void UpdateExerciseDialog()
    {
        var currentExercise = _workouts[_currentExerciseIndex];
        _exerciseNameTextBlock.Text = currentExercise.Exercise.exercise_name;
        _exerciseRepsTextBlock.Text = $"Reps: {currentExercise.Exercise.reps}";
        _exerciseVideoWebView.Source = new Uri(currentExercise.Exercise.linkvideo);

        _workoutDialog.SecondaryButtonText = _currentExerciseIndex == _workouts.Count - 1 ? "Finish" : "Next";
        _workoutDialog.IsPrimaryButtonEnabled = _currentExerciseIndex > 0;
    }

    private void NextExercise()
    {
        if (_currentExerciseIndex < _workouts.Count - 1)
        {
            _currentExerciseIndex++;
            UpdateExerciseDialog();
        }
    }

    private void PreviousExercise()
    {
        if (_currentExerciseIndex > 0)
        {
            _currentExerciseIndex--;
            UpdateExerciseDialog();
        }
    }


    public async Task ShowExerciseVideoDialogAsync(Exercise exercise)
    {
        // Create UI elements for the exercise
        var exerciseNameTextBlock = new TextBlock { FontSize = 24, FontWeight = FontWeights.Bold, Text = exercise.exercise_name };
        var exerciseRepsTextBlock = new TextBlock { FontSize = 18, Text = $"Reps: {exercise.reps}" };
        var exerciseVideoWebView = new WebView2 { Width = 900, Height = 320, HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };


        exerciseVideoWebView.Source = new Uri(exercise.linkvideo);

        // Arrange the elements in a Grid
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
        // Create the ContentDialog

        var titleTextBlock = new TextBlock
        {
            Text = "Bài Tập",
            FontSize = 24,
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

        exerciseDialog.CloseButtonStyle = new Style(typeof(Button))
        {
            Setters =
        {
            new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Center)
        }
        };
        exerciseDialog.Resources["ContentDialogMaxWidth"] = 2000;
        exerciseDialog.Resources["ContentDialogMaxHeight"] = 1500;
        // Set XamlRoot if using WinUI 3
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            exerciseDialog.XamlRoot = rootElement.XamlRoot;
        }

        // Ensure the dialog is shown on the UI thread
        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        await dispatcherQueue.EnqueueAsync(async () =>
        {
            await exerciseDialog.ShowAsync();
        });
    }

    public async Task<bool> ShowVerificationDialogAsync(string email, string code)
    {
        // Tạo Grid để sắp xếp nội dung
        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Title
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Email
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Description
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // TextBox
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Status
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Resend email

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

        var primaryButtonStyle = new Style(typeof(Button));
        primaryButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204)))); // Nền xanh (RGB: 0, 102, 204)
        primaryButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)))); // Chữ trắng
        primaryButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0))); // Không viền
        primaryButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(5))); // Bo góc
        primaryButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5))); // Căn chỉnh Padding

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

    public async Task<(string email, string username)> ShowUsernameInputDialog()
    {
        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Title
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // TextBox
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Status

        var titleTextBlock = new TextBlock
        {
            Text = "Nhập username tài khoản",
            FontSize = 24,
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(0, 0, 0, 10)
        };

        var usernameTextBox = new TextBox
        {
            PlaceholderText = "Tên đăng nhập",
            Margin = new Thickness(0, 0, 0, 10)
        };

        var statusTextBlock = new TextBlock
        {
            Text = string.Empty, // Trạng thái ban đầu trống
            FontSize = 14,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)), // Màu đỏ
            Visibility = Visibility.Collapsed, // Ẩn ban đầu
            Margin = new Thickness(0, 0, 0, 10)
        };

        grid.Children.Add(titleTextBlock);
        Grid.SetRow(titleTextBlock, 0);

        grid.Children.Add(usernameTextBox);
        Grid.SetRow(usernameTextBox, 1);

        grid.Children.Add(statusTextBlock);
        Grid.SetRow(statusTextBlock, 2);

        var primaryButtonStyle = new Style(typeof(Button));
        primaryButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204)))); // Nền xanh (RGB: 0, 102, 204)
        primaryButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)))); // Chữ trắng
        primaryButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0))); // Không viền
        primaryButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(5))); // Bo góc
        primaryButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5))); // Căn chỉnh Padding

        var closeButtonStyle = new Style(typeof(Button));
        closeButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204)))); // Nền xanh (RGB: 0, 102, 204)
        closeButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)))); // Chữ trắng
        closeButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0))); // Không viền
        closeButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(5))); // Bo góc
        closeButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5))); // Căn chỉnh Padding

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
            Text = string.Empty, // Trạng thái ban đầu trống
            FontSize = 14,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)), // Màu đỏ
            Visibility = Visibility.Collapsed, // Ẩn ban đầu
            Margin = new Thickness(0, 0, 0, 10)
        };

        var confirmPasswordBox = new PasswordBox
        {
            PlaceholderText = "Nhập lại mật khẩu mới",
            Margin = new Thickness(0, 0, 0, 10)
        };

        var confirmPasswordStatusTextBlock = new TextBlock
        {
            Text = string.Empty, // Trạng thái ban đầu trống
            FontSize = 14,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)), // Màu đỏ
            Visibility = Visibility.Collapsed, // Ẩn ban đầu
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
        primaryButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204)))); // Nền xanh (RGB: 0, 102, 204)
        primaryButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)))); // Chữ trắng
        primaryButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0))); // Không viền
        primaryButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(5))); // Bo góc
        primaryButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5))); // Căn chỉnh Padding

        var closeButtonStyle = new Style(typeof(Button));
        closeButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 102, 204)))); // Nền xanh (RGB: 0, 102, 204)
        closeButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)))); // Chữ trắng
        closeButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0))); // Không viền
        closeButtonStyle.Setters.Add(new Setter(Button.CornerRadiusProperty, new CornerRadius(5))); // Bo góc
        closeButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 5, 10, 5))); // Căn chỉnh Padding

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

    public async Task ShowErrorDialogAsync(string errorMessage)
    {
        var errorIcon = new FontIcon
        {
            Glyph = "\uEB90", // Mã Unicode của biểu tượng lỗi
            FontSize = 50,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)), // Màu đỏ
            Margin = new Thickness(10, 10, 10 ,10)
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

    public async Task<bool> ShowProductDialogWithSupplierAsync(Product product, int orderid)
    {
        bool addSuccess = true;
        // Hình ảnh sản phẩm
        var productImage = new Image
        {
            Source = new BitmapImage(new Uri("ms-appx:///Assets/planselection_model/expert.png")),
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
            Text = $"{product.product_price} đ",
            FontSize = 18,
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red),
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(10, 10, 10, 10)
        };

        // Đã bán
        var productSoldTextBlock = new TextBlock
        {
            Text = $"Đã bán: 20 (nhớ chỉnh)",
            FontSize = 16,
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.Gray),
            Margin = new Thickness(10, 10, 10, 10)
        };

        // Nhà cung cấp
        var supplierTextBlock = new TextBlock
        {
            Text = $"Nhà cung cấp: {product.product_provider}",
            FontSize = 16,
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black),
            Margin = new Thickness(10, 10, 10, 10)
        };

        // Số lượng sản phẩm với NumberBox
        var quantityLabel = new TextBlock
        {
            Text = "Số lượng:",
            FontSize = 16,
            Margin = new Thickness(10, 10, 10, 10)
        };

        var quantityNumberBox = new NumberBox
        {
            Minimum = 1,
            Maximum = 20,
            Value = 1,
            Width = 150,
            HorizontalAlignment = HorizontalAlignment.Left,
            SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline,
            Margin = new Thickness(10, 10, 10, 10)
        };

        var descriptionLabel = new TextBlock
        {
            Text = "Mô tả:",
            FontSize = 16,
            Margin = new Thickness(10, 10, 10, 10)
        };

        var descriptionTextBlock = new TextBlock
        {
            Text = product.description,
            FontSize = 15,
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
        rightPanel.Children.Add(productSoldTextBlock);
        rightPanel.Children.Add(supplierTextBlock); // Thêm mục Nhà cung cấp
        rightPanel.Children.Add(quantityLabel);
        rightPanel.Children.Add(quantityNumberBox);
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

        productDialog.Resources["ContentDialogMaxWidth"] = 1200;
        productDialog.Resources["ContentDialogMaxHeight"] = 600;

        // Xử lý sự kiện thêm vào giỏ hàng
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
            if(postResult == null)
            {
                addSuccess = false;
            }
        };

        // Thiết lập XamlRoot nếu dùng WinUI 3
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            productDialog.XamlRoot = rootElement.XamlRoot;
        }

        // Hiển thị dialog
        await productDialog.ShowAsync();
        return addSuccess;
    }
}