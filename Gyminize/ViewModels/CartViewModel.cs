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
using Twilio.Rest.Api.V2010.Account;
using Gyminize.Core.Services;


namespace Gyminize.ViewModels;


public partial class CartViewModel : ObservableRecipient
{

    private readonly INavigationService _navigationService;
    private readonly ILocalSettingsService _localSettingsService;
    private readonly IDialogService _dialogService;
    private readonly IApiServicesClient _apiServicesClient;

    private decimal _totalPayment;
    public decimal TotalPayment
    {
        get => _totalPayment;
        set => SetProperty(ref _totalPayment, value);
    }

    private int _totalProductCount;
    public int TotalProductCount
    {
        get => _totalProductCount;
        set => SetProperty(ref _totalProductCount, value);
    }

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

    public ICommand IncrementCommand
    {
        get;
    }
    public ICommand DecrementCommand
    {
        get;
    }


    public ObservableCollection<Orderdetail> OrderDetailsItems { get; set; } = new ObservableCollection<Orderdetail>();
    
    public Orders currentOrder = new Orders();
    public CartViewModel(INavigationService navigationService,ILocalSettingsService localSettingsService, IDialogService dialogService, IApiServicesClient apiServicesClient)
    {
        _navigationService = navigationService;
        _localSettingsService = localSettingsService;
        _dialogService = dialogService;
        _apiServicesClient = apiServicesClient;
        DeleteOrderDetailCommand = new AsyncRelayCommand<Orderdetail>(DeleteOrderDetailAsync);
        BuyingSelectedCommand = new RelayCommand(BuyingSelected);
        IncrementCommand = new RelayCommand<Orderdetail>(Increment);
        DecrementCommand = new RelayCommand<Orderdetail>(Decrement);

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
        {
            var orderlist = _apiServicesClient.Get<List<Orders>>($"api/Order/get/customerId/All/{CustomerId}");
            var filteredOrder = orderlist.FirstOrDefault(order => string.IsNullOrEmpty(order.status));
            if (filteredOrder != null)
            {
                OrderDetailsItems.Clear();
                foreach (var orderDetail in filteredOrder.Orderdetail)
                {
                    OrderDetailsItems.Add(orderDetail);
                }
                UpdateTotals();
                currentOrder = filteredOrder;
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: " + ex.Message);
            Debug.WriteLine($"Error loading order details: {ex.Message}");
        }
    }

    private void UpdateTotals()
    {
        TotalProductCount = OrderDetailsItems.Count;
        if (TotalProductCount != 0)
        {
            TotalPayment = OrderDetailsItems.Sum(item => item.detail_price);
        } else { TotalPayment = 0; }
    }

    private void Increment(Orderdetail orderDetail)
    {
        if (orderDetail != null)
        {
            orderDetail.product_amount++;
            var puttResult = ApiServices.Put<Orderdetail>("api/OrderDetail/update/number", orderDetail);
            if (puttResult != null)
            {
                var index = OrderDetailsItems.IndexOf(orderDetail);
                OrderDetailsItems[index] = orderDetail;
                OnPropertyChanged(nameof(OrderDetailsItems));
                UpdateTotals();
            }
        }
    }

    private void Decrement(Orderdetail orderDetail)
    {
        if (orderDetail != null && orderDetail.product_amount > 1)
        {
            orderDetail.product_amount--;
            var puttResult = ApiServices.Put<Orderdetail>("api/OrderDetail/update/number", orderDetail);
            if (puttResult != null)
            {
                var index = OrderDetailsItems.IndexOf(orderDetail);
                OrderDetailsItems[index] = orderDetail;
                OnPropertyChanged(nameof(OrderDetailsItems));
                UpdateTotals();
            }
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
                UpdateTotals();
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
    
    public async void BuyingSelected()
    {
        currentOrder.total_price = TotalPayment;
        if (TotalPayment != 0)
        {
            await _localSettingsService.SaveSettingAsync<Orders>("currentOrder", currentOrder);
            var pageKey = typeof(PaymentViewModel).FullName;
            if (pageKey != null)
            {
                _navigationService.NavigateTo(pageKey);
            }
        }else
        {
            await _dialogService.ShowErrorDialogAsync("Bạn chưa thêm sản phẩm nào vào giỏ hàng");
        }
    }
}
