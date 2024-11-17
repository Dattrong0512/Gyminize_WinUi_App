using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;
using Gyminize.Core.Services;
using Gyminize.Models;
using Gyminize.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Gyminize.ViewModels;

public partial class PlanSelectionViewModel : ObservableRecipient, INavigationAware
{
    
    private readonly INavigationService _navigationService;
    private readonly ILocalSettingsService _localSettingsService;
    private UIElement? _shell = null;
    public ICommand Plan1SelectedCommand
    {
        get; set;
    }

    public ICommand Plan2SelectedCommand
    {
        get; set;
    }

    public ICommand Plan3SelectedCommand
    {
        get; set;
    }

    public string customer_id;
    
    // Khởi tạo PlanViewModel.
    public PlanSelectionViewModel(INavigationService navigationService, ILocalSettingsService localSettingsService)
    {
        _navigationService = navigationService;
        _localSettingsService = localSettingsService;
        Plan1SelectedCommand = new RelayCommand(Plan1Selection);
        Plan2SelectedCommand = new RelayCommand(Plan2Selection);
        Plan3SelectedCommand = new RelayCommand(Plan3Selection);
    }

    public  void Plan1Selection()
    {
        var endpoint = "";
        endpoint = $"api/Plandetail/create/customer_id/" + customer_id + "/plan/1";
        var result = ApiServices.Post<Plandetail>(endpoint, null);
        var frame = new Frame();
        _shell = App.GetService<ShellPage>();
        frame.Content = _shell;
        App.MainWindow.Content = frame;
        _navigationService.NavigateTo(typeof(PlanViewModel).FullName);
    }
    public void Plan2Selection()
    {
        var endpoint = "";
        endpoint = $"api/Plandetail/create/customer_id/" + customer_id + "/plan/2";
        var result = ApiServices.Post<Plandetail>(endpoint, null);
        var frame = new Frame();
        _shell = App.GetService<ShellPage>();
        frame.Content = _shell;
        App.MainWindow.Content = frame;
        _navigationService.NavigateTo(typeof(PlanViewModel).FullName);
    }
    public void Plan3Selection()
    {
   
        var endpoint = "";
        endpoint = $"api/Plandetail/create/customer_id/" + customer_id + "/plan/3";
        var result = ApiServices.Post<Plandetail>(endpoint, null);
        var frame = new Frame();
        _shell = App.GetService<ShellPage>();
        frame.Content = _shell;
        App.MainWindow.Content = frame;
        _navigationService.NavigateTo(typeof(PlanViewModel).FullName);
    }

    public async void OnNavigatedTo(object parameter) 
    { 
        customer_id = await _localSettingsService.ReadSettingAsync<string>("customer_id");
    }
    public void OnNavigatedFrom()
    {
    

    }
}
