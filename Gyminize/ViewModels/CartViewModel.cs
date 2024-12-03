// ViewModel cho trang cửa hàng.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
// Thực hiện giao diện INavigationAware để nhận biết điều hướng.
using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Models;
using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;
using Gyminize.Core.Contracts.Services;
using Gyminize.Views;
using System.Diagnostics;


namespace Gyminize.ViewModels;


public partial class CartViewModel : ObservableRecipient
{

    private readonly INavigationService _navigationService;
    private readonly ILocalSettingsService _localSettingsService;
    private readonly IDialogService _dialogService;
    private readonly IApiServicesClient _apiServicesClient;

    private int _customerId;
    public int CustomerId
    {
        get => _customerId;
        set => SetProperty(ref _customerId, value);
    }

    public ObservableCollection<Product> ProductLibraryItems { get; set; } = new ObservableCollection<Product>();
    public ObservableCollection<Product> FilteredProductLibraryItems { get; set; } = new ObservableCollection<Product>();
    public ICommand DeleteOrderDetailCommand
    {
        get;
    }

    public ICommand BuyingSelectedCommand
    {
        get;
    }
    public List<Orderdetail> OrderDetailsItems { get; set; } = new List<Orderdetail>();
    public CartViewModel(INavigationService navigationService,ILocalSettingsService localSettingsService, IDialogService dialogService, IApiServicesClient apiServicesClient)
    {
        _navigationService = navigationService;
        _localSettingsService = localSettingsService;
        _dialogService = dialogService;
        _apiServicesClient = apiServicesClient;
        DeleteOrderDetailCommand = new AsyncRelayCommand<Orderdetail>(DeleteOrderDetailAsync);
        BuyingSelectedCommand = new RelayCommand(BuyingSelected);
        LoadOrderDetailData();
    }

    public async Task GetCustomerID()
    {
        var customer_id = await _localSettingsService.ReadSettingAsync<string>("customer_id");
        CustomerId = int.Parse(customer_id);
    }



    public async void LoadOrderDetailData()
    {
        await GetCustomerID();
        try
        {// sửa lại api xóa
            var orderlist = _apiServicesClient.Get<List<Orders>>($"api/Order/get/customerId/All/{CustomerId}");
            var filteredOrder = orderlist.FirstOrDefault(order => string.IsNullOrEmpty(order.status));
            Orders orders = filteredOrder;
            OrderDetailsItems = orders.Orderdetails.ToList();
            
        }
        catch (Exception ex)
        {
            _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: " + ex.Message);
            Debug.WriteLine($"Error loading order details: {ex.Message}");
        }
    }

    public async Task DeleteOrderDetailAsync(Orderdetail orderDetail)
    {
        if (orderDetail == null)
            return;
        // Gọi API để xóa FoodDetail
        try
        {

            var deleteResult = _apiServicesClient.Delete($"api/OrderDetail/delete", orderDetail);

            if (deleteResult)
            {
                OrderDetailsItems.Remove(orderDetail);
                // Cập nhật tính tổng tiền
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to delete FoodDetail.");
                await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: không thể xóa thức ăn");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error deleting FoodDetail: {ex.Message}");
            await _dialogService.ShowErrorDialogAsync($"Lỗi hệ thống: {ex.Message}");
        }
    }
    
    public void BuyingSelected()
    {
        var pageKey = typeof(PaymentViewModel).FullName;
        if (pageKey != null)
        {
            _navigationService.NavigateTo(pageKey);
        }
    }
}
