using Gyminize.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Views;

public sealed partial class PaymentPage : Page
{
    public PaymentViewModel ViewModel
    {
        get;
    }

    public PaymentPage()
    {
        ViewModel = App.GetService<PaymentViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }
}
