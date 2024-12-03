using Gyminize.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Views;

public sealed partial class CartPage : Page
{
    public CartViewModel ViewModel
    {
        get;
    }

    public CartPage()
    {
        ViewModel = App.GetService<CartViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }
}
