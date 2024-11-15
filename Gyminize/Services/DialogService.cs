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
}