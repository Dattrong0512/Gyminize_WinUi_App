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
    }
}
