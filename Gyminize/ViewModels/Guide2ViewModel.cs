using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Gyminize.ViewModels;
public class Guide2ViewModel : ObservableRecipient
{
    private Border? _selectedBorder;

    public ICommand PointerEnteredCommand { get; }
    public ICommand PointerExitedCommand { get; }
    public ICommand PointerPressedCommand { get; }
    public ICommand PointerReleasedCommand { get; }

    public Guide2ViewModel()
    {
        PointerEnteredCommand = new RelayCommand<Border?>(OnPointerEntered);
        PointerExitedCommand = new RelayCommand<Border?>(OnPointerExited);
        PointerPressedCommand = new RelayCommand<Border?>(OnPointerPressed);
        PointerReleasedCommand = new RelayCommand<Border?>(OnPointerReleased);
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
}
