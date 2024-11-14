using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using CommunityToolkit.WinUI;
using Gyminize.Contracts.Services;
using Gyminize.Helpers;

using Gyminize;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Text;

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
    private List<WorkoutDetail> _workouts = new List<WorkoutDetail>();
    private TextBlock _exerciseNameTextBlock;
    private TextBlock _exerciseRepsTextBlock;
    private WebView2 _exerciseVideoWebView;
    private ContentDialog _workoutDialog;
    public async Task<bool> ShowFullExerciseWorkoutDialogAsync(List<WorkoutDetail> workouts)
    {
        _workouts = workouts;
        _currentExerciseIndex = 0;
        bool isFinished = false;

        _workoutDialog = new ContentDialog
        {
            Title = "Danh sách phát bài tập",
            PrimaryButtonText = "Next",
            SecondaryButtonText = "Previous",
            CloseButtonText = "Kết thúc",
            Width = 400,
            Height = 500
        };

        // Thiết lập XamlRoot nếu dùng WinUI 3
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            _workoutDialog.XamlRoot = rootElement.XamlRoot;
        }
        // Tạo UI cho bài tập hiện tại
        _exerciseNameTextBlock = new TextBlock { FontSize = 24, FontWeight = FontWeights.Bold };
        _exerciseRepsTextBlock = new TextBlock { FontSize = 18 };
        _exerciseVideoWebView = new WebView2 { Width = 300, Height = 200 };

        // Sắp xếp các thành phần trong StackPanel
        var stackPanel = new StackPanel { Spacing = 20 };
        stackPanel.Children.Add(_exerciseNameTextBlock);
        stackPanel.Children.Add(_exerciseRepsTextBlock);
        stackPanel.Children.Add(_exerciseVideoWebView);
        _workoutDialog.Content = stackPanel;

        UpdateExerciseDialog();

        // Đảm bảo gọi trên UI Thread
        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        await dispatcherQueue.EnqueueAsync(async () =>
        {
            ContentDialogResult result;
            do
            {
                result = await _workoutDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
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
                else if (result == ContentDialogResult.Secondary)
                {
                    PreviousExercise();
                }
                else
                {
                    isFinished = false;
                }
            } while (result != ContentDialogResult.None); // "Kết thúc" để thoát
        });
        return isFinished;
    }

    private void UpdateExerciseDialog()
    {
        var currentExercise = _workouts[_currentExerciseIndex];
        _exerciseNameTextBlock.Text = currentExercise.ExerciseDetails[_currentExerciseIndex].ExerciseName;
        _exerciseRepsTextBlock.Text = $"Reps: {currentExercise.ExerciseDetails[_currentExerciseIndex].Reps}";
        _exerciseVideoWebView.Source = new Uri(currentExercise.ExerciseDetails[_currentExerciseIndex].LinkVideo);

        _workoutDialog.PrimaryButtonText = _currentExerciseIndex == _workouts.Count - 1 ? "Finish" : "Next";
        _workoutDialog.IsSecondaryButtonEnabled = _currentExerciseIndex > 0;

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

}