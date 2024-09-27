using Gyminize.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Views;

public sealed partial class PlanPage : Page
{
    public PlanViewModel ViewModel
    {
        get;
    }

    public PlanPage()
    {
        ViewModel = App.GetService<PlanViewModel>();
        InitializeComponent();
    }
}
