using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Gyminize.Contracts.Services;
using Gyminize.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Gyminize.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class GuidePage3 : Page
{
    public Guide3ViewModel ViewModel
    {
        get;
    }

    public GuidePage3()
    {
        ViewModel = App.GetService<Guide3ViewModel>();
        InitializeComponent();
        var windowService = App.GetService<IWindowService>();
        var navigationService = App.GetService<INavigationService>();
        var setting  = App.GetService<ILocalSettingsService>();
        ViewModel = new Guide3ViewModel(navigationService, windowService, setting);
        this.DataContext = ViewModel;
    }
}
