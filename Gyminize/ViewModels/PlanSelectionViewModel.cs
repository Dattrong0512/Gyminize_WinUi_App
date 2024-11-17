using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;

namespace Gyminize.ViewModels;

public partial class PlanSelectionViewModel : ObservableRecipient
{

    private INavigationService _navigationService;
    private ILocalSettingsService _localSettingsService;
    ICommand Plan1SelectionCommand
    {
        get; set;
    }

    ICommand Plan2SelectionCommand
    {
        get; set;
    }

    ICommand Plan3SelectionCommand
    {
        get; set;
    }

    public int customer_id;
    // Khởi tạo PlanViewModel.
    public PlanSelectionViewModel(INavigationService navigationService, ILocalSettingsService localSettingsService)
    {
        _navigationService = navigationService;
        _localSettingsService = localSettingsService;
        Plan1SelectionCommand = new RelayCommand(Plan1Selection);
        Plan2SelectionCommand = new RelayCommand(Plan2Selection);
        Plan3SelectionCommand = new RelayCommand(Plan3Selection);
    }

    public void Plan1Selection()
    {
        
    }
    public void Plan2Selection() { }
    public void Plan3Selection()
    {
    }
}
