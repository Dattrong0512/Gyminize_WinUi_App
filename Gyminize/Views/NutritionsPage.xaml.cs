using Gyminize.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Gyminize.Models;
namespace Gyminize.Views;

public sealed partial class NutritionsPage : Page
{
    public NutritionsViewModel ViewModel { get; }

    public NutritionsPage()
    {
        ViewModel = App.GetService<NutritionsViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    private async void AddButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        // Lấy thực phẩm được chọn từ Tag
        var selectedFood = (sender as Button).Tag as FoodItem;

        // Tạo dialog để hỏi người dùng thêm thực phẩm vào bữa ăn nào
        ContentDialog mealDialog = new ContentDialog
        {
            Title = "Chọn Bữa Ăn",
            Content = new ComboBox
            {
                Items = { "Bữa Sáng", "Bữa Trưa", "Bữa Tối", "Bữa Xế" },
                SelectedIndex = 0
            },
            PrimaryButtonText = "Thêm",
            CloseButtonText = "Hủy",
            XamlRoot = this.Content.XamlRoot // Ensure the dialog is associated with the current XAML root
        };

        // Hiển thị dialog và chờ kết quả
        ContentDialogResult result = await mealDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            ComboBox comboBox = mealDialog.Content as ComboBox;
            string selectedMeal = comboBox.SelectedItem.ToString();

            // Thêm thực phẩm vào bữa ăn được chọn
            if (selectedMeal == "Bữa Sáng")
            {
                ViewModel.BreakfastItems.Add(selectedFood);
                BreakfastItemsControl.Items.Add(selectedFood);
            }
            else if (selectedMeal == "Bữa Trưa")
            {
                ViewModel.LunchItems.Add(selectedFood);
                LunchItemsControl.Items.Add(selectedFood);
            }
            else if (selectedMeal == "Bữa Tối")
            {
                ViewModel.DinnerItems.Add(selectedFood);
                DinnerItemsControl.Items.Add(selectedFood);
            }
            else if (selectedMeal == "Bữa Xế")
            {
                ViewModel.SnackItems.Add(selectedFood);
                SnackItemsControl.Items.Add(selectedFood);
            }
        }
    }
}
