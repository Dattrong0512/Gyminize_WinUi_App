using Gyminize.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Gyminize.Models;
using Gyminize.Contracts.Services;

namespace Gyminize.Views
{
    public sealed partial class NutritionsPage : Page
    {
        public NutritionsViewModel ViewModel
        {
            get;
        }

        public NutritionsPage()
        {
            ViewModel = App.GetService<NutritionsViewModel>();
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            ViewModel.SearchFoodCommand.Execute(textBox.Text);
        }
    }
}
