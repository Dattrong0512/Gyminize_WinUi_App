using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;
using Gyminize.Models;
using Gyminize.Services;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace Gyminize.ViewModels;
public class Guide2ViewModel : ObservableRecipient, INavigationAware
{
    private Border? _selectedBorder;
    private CustomerInfo _customerInfo;

    public ICommand PointerEnteredCommand { get; }
    public ICommand PointerExitedCommand { get; }
    public ICommand PointerPressedCommand { get; }
    public ICommand PointerReleasedCommand { get; }

    public ICommand NavigateBackCommand
    {
        get;
    }
    private INavigationService _navigationService;
    private string _imageSource1;
    public string ImageSource1
    {
        get => _imageSource1;
        set => SetProperty(ref _imageSource1, value);
    }

    private string _imageSource2;
    public string ImageSource2
    {
        get => _imageSource2;
        set => SetProperty(ref _imageSource2, value);
    }

    private string _imageSource3;
    public string ImageSource3
    {
        get => _imageSource3;
        set => SetProperty(ref _imageSource3, value);
    }

    private string _imageSource4;
    public string ImageSource4
    {
        get => _imageSource4;
        set => SetProperty(ref _imageSource4, value);
    }

    private string _imageSource5;
    public string ImageSource5
    {
        get => _imageSource5;
        set => SetProperty(ref _imageSource5, value);
    }

    public Guide2ViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        PointerEnteredCommand = new RelayCommand<Border?>(OnPointerEntered);
        PointerExitedCommand = new RelayCommand<Border?>(OnPointerExited);
        PointerPressedCommand = new RelayCommand<Border?>(OnPointerPressed);
        PointerReleasedCommand = new RelayCommand<Border?>(OnPointerReleased);
        NavigateBackCommand = new RelayCommand(NavigateBack);
    }

    private void OnPointerEntered(Border? border)
    {
        if (border != null && border != _selectedBorder)
        {
            border.BorderBrush = new SolidColorBrush(Colors.Blue);
            border.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 81, 93, 239)); // #515DEF
        }
    }

    private void OnPointerExited(Border? border)
    {
        if (border != null && border != _selectedBorder)
        {
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.Background = new SolidColorBrush(Colors.Transparent);
        }
    }

    private void OnPointerPressed(Border? border)
    {
        if (border != null)
        {
            if (_selectedBorder != null)
            {
                // Reset the previously selected border
                _selectedBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                _selectedBorder.Background = new SolidColorBrush(Colors.Transparent);
            }

            // Set the new selected border
            _selectedBorder = border;
            border.BorderBrush = new SolidColorBrush(Colors.DarkBlue);
            border.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 61, 73, 189));
        }
    }

    private void OnPointerReleased(Border? border)
    {
        if (border != null && border == _selectedBorder)
        {
            border.BorderBrush = new SolidColorBrush(Colors.Blue);
            border.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 81, 93, 239));
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is CustomerInfo customerInfo)
        {
            _customerInfo = customerInfo;
            // Set image sources based on gender
            if (_customerInfo.sex == 1) // Male
            {
                ImageSource1 = "ms-appx:///Assets/bodyfat_male5.png";
                ImageSource2 = "ms-appx:///Assets/bodyfat_male4.png";
                ImageSource3 = "ms-appx:///Assets/bodyfat_male3.png";
                ImageSource4 = "ms-appx:///Assets/bodyfat_male2.png";
                ImageSource5 = "ms-appx:///Assets/bodyfat_male1.png";
            }
            else // Female
            {
                ImageSource1 = "ms-appx:///Assets/bodyfat_female_5.png";
                ImageSource2 = "ms-appx:///Assets/bodyfat_female_4.png";
                ImageSource3 = "ms-appx:///Assets/bodyfat_female_3.png";
                ImageSource4 = "ms-appx:///Assets/bodyfat_female_2.png";
                ImageSource5 = "ms-appx:///Assets/bodyfat_female_1.png";
            }
        }
    }

    public void OnNavigatedFrom()
    {
        // Perform any necessary actions when navigating away from the page
    }

    private void NavigateBack()
    {
        var pageKey = typeof(Guide1ViewModel).FullName;
        if (pageKey != null)
        {
            _navigationService.NavigateTo(pageKey, _customerInfo);
        }
    }
}
