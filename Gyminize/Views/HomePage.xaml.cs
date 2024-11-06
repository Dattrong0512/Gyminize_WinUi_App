using Gyminize.Contracts.Services;
using Gyminize.ViewModels;


using Microsoft.UI.Xaml.Controls;
using System;


namespace Gyminize.Views;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
        var windowService = App.GetService<IWindowService>();
        var navigationService = App.GetService<INavigationService>();
        var setting = App.GetService<ILocalSettingsService>();
        ViewModel = new HomeViewModel(navigationService,windowService, setting);
        this.DataContext = ViewModel;
    }

    private void OnWorkoutBorderTapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        var viewModel = (HomeViewModel)this.DataContext;
        viewModel.OpenWorkoutLinkCommand.Execute(null);
    }

    private void OnSleepBorderTapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        var viewModel = (HomeViewModel)this.DataContext;
        viewModel.OpenSleepLinkCommand.Execute(null);
    }

    private void OnRecipeBorderTapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        var viewModel = (HomeViewModel)this.DataContext;
        viewModel.OpenRecipeLinkCommand.Execute(null);
    }
}
