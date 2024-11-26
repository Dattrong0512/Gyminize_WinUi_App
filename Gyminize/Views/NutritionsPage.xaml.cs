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
    }
}
