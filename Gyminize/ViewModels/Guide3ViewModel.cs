using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;
using Gyminize.Models;
using Microsoft.UI.Xaml.Media;
using Gyminize.Helpers;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
namespace Gyminize.ViewModels;
public class Guide3ViewModel : ObservableRecipient, INavigationAware
{
    private CustomerInfo _customerInfo;
    private INavigationService _navigationService;
    private double _bmi;
    public double BMIStat
    {
        get => _bmi;
        set => SetProperty(ref _bmi, value);
    }
    private double _tdde;
    public double TDEEStat
    {
        get => _tdde;
        set => SetProperty(ref _tdde, value);
    }

    private double _cuttingProteins;
    public double CuttingProteins
    {
        get => _cuttingProteins;
        set => SetProperty(ref _cuttingProteins, value);
    }

    private double _cuttingCarbs;
    public double CuttingCarbs
    {
        get => _cuttingCarbs;
        set => SetProperty(ref _cuttingCarbs, value);
    }

    private double _cuttingFats;
    public double CuttingFats
    {
        get => _cuttingFats;
        set => SetProperty(ref _cuttingFats, value);
    }

    private double _maintenanceProteins;
    public double MaintenanceProteins
    {
        get => _maintenanceProteins;
        set => SetProperty(ref _maintenanceProteins, value);
    }

    private double _maintenanceCarbs;
    public double MaintenanceCarbs
    {
        get => _maintenanceCarbs;
        set => SetProperty(ref _maintenanceCarbs, value);
    }

    private double _maintenanceFats;
    public double MaintenanceFats
    {
        get => _maintenanceFats;
        set => SetProperty(ref _maintenanceFats, value);
    }

    private double _bulkingProteins;
    public double BulkingProteins
    {
        get => _bulkingProteins;
        set => SetProperty(ref _bulkingProteins, value);
    }

    private double _bulkingCarbs;
    public double BulkingCarbs
    {
        get => _bulkingCarbs;
        set => SetProperty(ref _bulkingCarbs, value);
    }

    private double _bulkingFats;

    public double BulkingFats
    {
        get => _bulkingFats;
        set => SetProperty(ref _bulkingFats, value);
    }

    private string _healthStatus;
    public string HealthStatus
    {
        get => _healthStatus;
        set => SetProperty(ref _healthStatus, value);
    }
    public ICommand NavigateNextCommand
    {
        get;
    }
    public ICommand NavigateBackCommand
    {
        get;
    }
    public Guide3ViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        NavigateBackCommand = new RelayCommand(NavigateBack);
        NavigateNextCommand = new RelayCommand(NavigateNext);
    }


    public void OnNavigatedTo(object parameter)
    {
        if (parameter is CustomerInfo customerInfo)
        {
            _customerInfo = customerInfo;
            TDEEStat = Math.Round(HealthCalculator.CalculateTDEE(_customerInfo.Weight, _customerInfo.BodyFat, _customerInfo.ActivityLevel),0);
            BMIStat = Math.Round(HealthCalculator.CalculateBMI(_customerInfo.Weight, _customerInfo.Height),1);
            _customerInfo.Tdee = (int)TDEEStat;
            MaintenanceProteins = Math.Round(HealthCalculator.CalculateNutritionGram("proteins","m",30,_tdde), 0);
            MaintenanceCarbs = Math.Round(HealthCalculator.CalculateNutritionGram("carbs", "m", 35, _tdde), 0);
            MaintenanceFats = Math.Round(HealthCalculator.CalculateNutritionGram("fats", "m", 35, _tdde), 0);
            BulkingProteins = Math.Round(HealthCalculator.CalculateNutritionGram("proteins", "b", 30, _tdde), 0);
            BulkingCarbs = Math.Round(HealthCalculator.CalculateNutritionGram("carbs", "b", 35, _tdde), 0);
            BulkingFats = Math.Round(HealthCalculator.CalculateNutritionGram("fats", "b", 35, _tdde), 0);
            CuttingProteins = Math.Round(HealthCalculator.CalculateNutritionGram("proteins", "c", 30, _tdde), 0);
            CuttingCarbs = Math.Round(HealthCalculator.CalculateNutritionGram("carbs", "c", 35, _tdde), 0);
            CuttingFats = Math.Round(HealthCalculator.CalculateNutritionGram("fats", "c", 35, _tdde), 0);
            HealthStatus = HealthCalculator.HealthStatus(BMIStat);
        }
    }

    public void OnNavigatedFrom()
    {
        // Perform any necessary actions when navigating away from the page
    }

    private void NavigateBack()
    {
        var pageKey = typeof(Guide2ViewModel).FullName;
        if (pageKey != null)
        {
            _navigationService.NavigateTo(pageKey, _customerInfo);
        }
    }

    private void NavigateNext()
    {
    }
}

