using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Gyminize.ViewModels;
using Gyminize.Contracts.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Gyminize.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class GuidePage1 : Page
{
    public Guide1ViewModel ViewModel { get; }
    
    public GuidePage1()
    {
        ViewModel = App.GetService<Guide1ViewModel>();
        InitializeComponent();
        var windowService = App.GetService<IWindowService>();
        var navigationService = App.GetService<INavigationService>();
        ViewModel = new Guide1ViewModel(navigationService, windowService);
        this.DataContext = ViewModel;
    }

    
    private void AgeTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (DataContext is Guide1ViewModel viewModel)
        {
            viewModel.AgeLostFocusCommand.Execute(null);
        }
    }

    private void HeightTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (DataContext is Guide1ViewModel viewModel)
        {
            viewModel.HeightLostFocusCommand.Execute(null);
        }
    }

    private void WeightTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (DataContext is Guide1ViewModel viewModel)
        {
            viewModel.WeightLostFocusCommand.Execute(null);
        }
    }

    

    private void maleCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (DataContext is Guide1ViewModel viewModel)
        {
            viewModel.MaleCheckCommand.Execute(null);
        }
    }

    private void femaleCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (DataContext is Guide1ViewModel viewModel)
        {
            viewModel.FemaleCheckCommand.Execute(null);
        }
    }

   
}
