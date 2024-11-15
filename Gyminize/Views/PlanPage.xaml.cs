using Gyminize.Contracts.Services;
using Gyminize.Models;
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
        var navigationService = App.GetService<INavigationService>();
        var dialogService = App.GetService<IDialogService>();
        ViewModel = new PlanViewModel(navigationService, dialogService);
        DataContext = ViewModel;

    }

    private void DayBorder_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        if (sender is Border border && border.DataContext is Workoutdetail workoutDetail)
        {
            ViewModel.SelectWorkoutDetailCommand.Execute(workoutDetail);
        }
    }
}
