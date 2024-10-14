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
    private CustomerInfo _customerInfo = new CustomerInfo(); 
    private string _imageSource1 = string.Empty; 
    private string _imageSource2 = string.Empty; 
    private string _imageSource3 = string.Empty; 
    private string _imageSource4 = string.Empty; 
    private string _imageSource5 = string.Empty; 
    private string _selectedImageSource = string.Empty;
    private Brush _imageBorder1Background = new SolidColorBrush(Colors.Transparent);
    private Brush _imageBorder2Background = new SolidColorBrush(Colors.Transparent);
    private Brush _imageBorder3Background = new SolidColorBrush(Colors.Transparent);
    private Brush _imageBorder4Background = new SolidColorBrush(Colors.Transparent);
    private Brush _imageBorder5Background = new SolidColorBrush(Colors.Transparent);


    public ICommand PointerEnteredCommand { get; }
    public ICommand PointerExitedCommand { get; }
    public ICommand PointerPressedCommand { get; }
    public ICommand PointerReleasedCommand { get; }

    public ICommand NavigateBackCommand
    {
        get;
    }
    private INavigationService _navigationService;
    public string ImageSource1
    {
        get => _imageSource1;
        set => SetProperty(ref _imageSource1, value);
    }

    public string ImageSource2
    {
        get => _imageSource2;
        set => SetProperty(ref _imageSource2, value);
    }

    public string ImageSource3
    {
        get => _imageSource3;
        set => SetProperty(ref _imageSource3, value);
    }

    public string ImageSource4
    {
        get => _imageSource4;
        set => SetProperty(ref _imageSource4, value);
    }

    public string ImageSource5
    {
        get => _imageSource5;
        set => SetProperty(ref _imageSource5, value);
    }

    public Brush ImageBorder1Background
    {
        get => _imageBorder1Background;
        set => SetProperty(ref _imageBorder1Background, value);
    }

    public Brush ImageBorder2Background
    {
        get => _imageBorder2Background;
        set => SetProperty(ref _imageBorder2Background, value);
    }

    public Brush ImageBorder3Background
    {
        get => _imageBorder3Background;
        set => SetProperty(ref _imageBorder3Background, value);
    }

    public Brush ImageBorder4Background
    {
        get => _imageBorder4Background;
        set => SetProperty(ref _imageBorder4Background, value);
    }

    public Brush ImageBorder5Background
    {
        get => _imageBorder5Background;
        set => SetProperty(ref _imageBorder5Background, value);
    }

    public string SelectedImageSource
    {
        get => _selectedImageSource;
        set => SetProperty(ref _selectedImageSource, value);
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
                _selectedBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                _selectedBorder.Background = new SolidColorBrush(Colors.Transparent);
            }

            // Set the new selected border
            _selectedBorder = border;
            border.BorderBrush = new SolidColorBrush(Colors.DarkBlue);
            border.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 61, 73, 189));

            if (border.Tag is string imageSource)
            {
                SelectedImageSource = imageSource;
                _customerInfo.BodyFat = GetBodyFatFromImgSource(imageSource);
            }
        }
    }

    private double GetBodyFatFromImgSource(string imgsrc)
    {
        int lastSlashIndex = imgsrc.LastIndexOf('/');
        string fileNameWithExtension = imgsrc.Substring(lastSlashIndex + 1);
        int lastDotIndex = fileNameWithExtension.LastIndexOf('.');
        string fileNameWithoutExtension = fileNameWithExtension.Substring(0, lastDotIndex);

        switch (fileNameWithoutExtension)
        {
            case "m_1":
                return 31.5;
            case "m_2":
                return 23.5;
            case "m_3":
                return 16.5;
            case "m_4":
                return 12.0;
            case "m_5":
                return 7.5;
            case "f_1":
                return 31.5;
            case "f_2":
                return 27.0;
            case "f_3":
                return 20.5;
            case "f_4":
                return 16.5;
            case "f_5":
                return 14.5;
            default:
                return 0.0;

        }
    }

    private string GetImgSourceFromBodyFat(double bodyFat, bool isMale)
    {
        string prefix = isMale ? "m_" : "f_";
        string fileName = bodyFat switch
        {
            31.5 => $"{prefix}1.png",
            23.5 => $"{prefix}2.png",
            16.5 when isMale => $"{prefix}3.png",
            16.5 when !isMale => $"{prefix}4.png",
            12.0 => $"{prefix}4.png",
            7.5 => $"{prefix}5.png",
            27.0 => $"{prefix}2.png",
            20.5 => $"{prefix}3.png",
            14.5 => $"{prefix}5.png",
            _ => throw new ArgumentException("Invalid body fat percentage")
        };

        return $"ms-appx:///Assets/BodyFat/{fileName}";
    }
    private void HighlightSelectedImage()
    {
        if (SelectedImageSource == ImageSource1)
        {
            ImageBorder1Background = new SolidColorBrush(ColorHelper.FromArgb(255, 61, 73, 189));
        }
        else if (SelectedImageSource == ImageSource2)
        {
            ImageBorder2Background = new SolidColorBrush(ColorHelper.FromArgb(255, 61, 73, 189));
        }
        else if (SelectedImageSource == ImageSource3)
        {
            ImageBorder3Background = new SolidColorBrush(ColorHelper.FromArgb(255, 61, 73, 189));
        }
        else if (SelectedImageSource == ImageSource4)
        {
            ImageBorder4Background = new SolidColorBrush(ColorHelper.FromArgb(255, 61, 73, 189));
        }
        else if (SelectedImageSource == ImageSource5)
        {
            ImageBorder5Background = new SolidColorBrush(ColorHelper.FromArgb(255, 61, 73, 189));
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
                ImageSource1 = "ms-appx:///Assets/BodyFat/m_5.png";
                ImageSource2 = "ms-appx:///Assets/BodyFat/m_4.png";
                ImageSource3 = "ms-appx:///Assets/BodyFat/m_3.png";
                ImageSource4 = "ms-appx:///Assets/BodyFat/m_2.png";
                ImageSource5 = "ms-appx:///Assets/BodyFat/m_1.png";
            }
            else // Female
            {
                ImageSource1 = "ms-appx:///Assets/BodyFat/f_5.png";
                ImageSource2 = "ms-appx:///Assets/BodyFat/f_4.png";
                ImageSource3 = "ms-appx:///Assets/BodyFat/f_3.png";
                ImageSource4 = "ms-appx:///Assets/BodyFat/f_2.png";
                ImageSource5 = "ms-appx:///Assets/BodyFat/f_1.png";
            }
            if (_customerInfo.BodyFat != 0)
            {
                SelectedImageSource = GetImgSourceFromBodyFat(_customerInfo.BodyFat, _customerInfo.sex == 1);
                HighlightSelectedImage();
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
