using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using Gyminize.Contracts.Services;
using Gyminize.Services;
using Gyminize.Models;
using Gyminize.Contracts.ViewModels;

namespace Gyminize.ViewModels;

/// <summary>
/// ViewModel cho Order, cung cấp các thuộc tính và phương thức để quản lý đơn hàng.
/// </summary>
public partial class OrderViewModel : ObservableRecipient, INavigationAware
{
    private IApiServicesClient _apiServicesClient;
    private ILocalSettingsService _localSettingsService;

    /// <summary>
    /// ID của khách hàng.
    /// </summary>
    public int CustomerId
    {
        get; set;
    }

    /// <summary>
    /// Nguồn dữ liệu cho danh sách đơn hàng.
    /// </summary>
    public ObservableCollection<Orders> Source { get; } = new ObservableCollection<Orders>();

    /// <summary>
    /// Khởi tạo một thể hiện mới của lớp <see cref="OrderViewModel"/>.
    /// </summary>
    /// <param name="apiServicesClient">Dịch vụ API.</param>
    /// <param name="localSettingsService">Dịch vụ cài đặt cục bộ.</param>
    public OrderViewModel(IApiServicesClient apiServicesClient, ILocalSettingsService localSettingsService)
    {
        _apiServicesClient = apiServicesClient;
        _localSettingsService = localSettingsService;
    }

    /// <summary>
    /// Lấy ID của khách hàng từ cài đặt cục bộ.
    /// </summary>
    /// <returns>Một tác vụ đại diện cho thao tác không đồng bộ.</returns>
    public async Task GetCustomerID()
    {
        var customer_id = await _localSettingsService.ReadSettingAsync<string>("customer_id");
        CustomerId = int.Parse(customer_id);
    }

    /// <summary>
    /// Phương thức được gọi khi điều hướng đến trang.
    /// </summary>
    /// <param name="parameter">Dữ liệu được truyền khi điều hướng.</param>
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

    /// <summary>
    /// Phương thức được gọi khi điều hướng đi từ trang.
    /// </summary>
    public void OnNavigatedFrom()
    {
    }
}
