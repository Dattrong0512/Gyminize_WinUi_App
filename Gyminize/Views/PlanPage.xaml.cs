using Gyminize.Contracts.Services;
using Gyminize.Helpers;
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
        ViewModel = new PlanViewModel(navigationService);
        DataContext = ViewModel;

    }

    private void DayBorder_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        if (sender is Border border && border.DataContext is WorkoutDetail workoutDetail)
        {
            ViewModel.SelectWorkoutDetailCommand.Execute(workoutDetail);
        }
    }
}
