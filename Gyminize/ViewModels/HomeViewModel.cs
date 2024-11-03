// ViewModel cho trang chủ.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Gyminize.Models;
using Gyminize.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Gyminize.Contracts.ViewModels;
using System.Diagnostics;
using System.Windows.Input;
namespace Gyminize.ViewModels;

public partial class HomeViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private bool _isWeightTextBoxEnabled;
    public bool IsWeightTextBoxEnabled
    {
        get => _isWeightTextBoxEnabled;
        private set => SetProperty(ref _isWeightTextBoxEnabled, value);
    }

    private int _weight;

    public string WeightText
    {
        get => _weight.ToString();
        set
        {
            if (int.TryParse(value, out int result))
            {
                _weight = result;
            }
            else
            {
                _weight = 0;
            }
            OnPropertyChanged();
        }
    }

    public RelayCommand OpenWorkoutLinkCommand
    {
        get;
    }
    public RelayCommand OpenSleepLinkCommand
    {
        get;
    }
    public RelayCommand OpenRecipeLinkCommand
    {
        get;
    }
    public ICommand EditWeightCommand
    {
        get;
    }

    public HomeViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        OpenWorkoutLinkCommand = new RelayCommand(OpenWorkoutLink);
        OpenSleepLinkCommand = new RelayCommand(OpenSleepLink);
        OpenRecipeLinkCommand = new RelayCommand(OpenRecipeLink);
        EditWeightCommand = new RelayCommand(OpenEditWeight);
        IsWeightTextBoxEnabled = false;
        WeightText = "80";
    }

    private void OpenWorkoutLink()
    {
        var uri = new Uri("https://darebee.com/workouts.html");
        Process.Start(new ProcessStartInfo
        {
            FileName = uri.AbsoluteUri,
            UseShellExecute = true
        });
    }

    private void OpenRecipeLink()
    {
        var uri = new Uri("https://www.allrecipes.com/recipes/");
        Process.Start(new ProcessStartInfo
        {
            FileName = uri.AbsoluteUri,
            UseShellExecute = true
        });
    }

    private void OpenSleepLink()
    {
        var uri = new Uri("https://sleepdoctor.com/");
        Process.Start(new ProcessStartInfo
        {
            FileName = uri.AbsoluteUri,
            UseShellExecute = true
        });
    }

    private void OpenEditWeight()
    {
        IsWeightTextBoxEnabled = true;
    }

    public void OnNavigatedTo(object parameter)
    {
    }

    public void OnNavigatedFrom()
    {
    }
}
