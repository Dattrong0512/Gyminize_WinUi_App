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
using Microsoft.UI;
using Gyminize.Contracts.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Gyminize.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GuidePage2 : Page
    {
        public Guide2ViewModel ViewModel { get; }

        public GuidePage2()
        {
            ViewModel = App.GetService<Guide2ViewModel>();
            InitializeComponent();
            var windowService = App.GetService<IWindowService>();
            var navigationService = App.GetService<INavigationService>();
            ViewModel = new Guide2ViewModel(navigationService, windowService);
            this.DataContext = ViewModel;
        }

        private void Border_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                ViewModel.PointerEnteredCommand.Execute(border);
            }
        }

        private void Border_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                ViewModel.PointerExitedCommand.Execute(border);
            }
        }

        private void Border_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                ViewModel.PointerPressedCommand.Execute(border);
            }
        }

        private void Border_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                ViewModel.PointerReleasedCommand.Execute(border);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.OnNavigatedTo(e.Parameter);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel.OnNavigatedFrom();
        }

        
    }
}
