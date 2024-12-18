using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using Gyminize.Contracts.Services;
using Gyminize.Services;
using Gyminize.Models;
using Gyminize.Contracts.ViewModels;

namespace Gyminize.ViewModels;

public partial class OrderViewModel : ObservableRecipient, INavigationAware
{
    private IApiServicesClient _apiServicesClient;
    private ILocalSettingsService _localSettingsService;

    public int CustomerId
    {
        get; set;
    }

    public ObservableCollection<Orders> Source { get; } = new ObservableCollection<Orders>();

    public OrderViewModel(IApiServicesClient apiServicesClient, ILocalSettingsService localSettingsService)
    {
        _apiServicesClient = apiServicesClient;
        _localSettingsService = localSettingsService;
    }

    public async Task GetCustomerID()
    {
        var customer_id = await _localSettingsService.ReadSettingAsync<string>("customer_id");
        CustomerId = int.Parse(customer_id);
    }

    public async void OnNavigatedTo(object parameter)
    {
        await GetCustomerID();
        Source.Clear();
        var orderlist = _apiServicesClient.Get<List<Orders>>($"api/Order/get/customerID/All/" + CustomerId);
        var filteredOrder = orderlist.Where(order => order.status.Contains("Completed"));
        if (filteredOrder != null)
        {
            foreach (var item in filteredOrder)
            {
                Source.Add(item);
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
