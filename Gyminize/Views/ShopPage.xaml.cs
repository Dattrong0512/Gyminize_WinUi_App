using Gyminize.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Views;

public sealed partial class ShopPage : Page
{
    public ShopViewModel ViewModel
    {
        get;
    }

    public ShopPage()
    {
        ViewModel = App.GetService<ShopViewModel>();
        InitializeComponent();
    }
}
