using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using CommunityToolkit.WinUI;
using Gyminize.Contracts.Services;
using Gyminize;

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
}