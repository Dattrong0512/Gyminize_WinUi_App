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
using Gyminize.Services;


namespace Gyminize.ViewModels;

public partial class ShopViewModel : ObservableRecipient
{
    private const int ItemsPerPage = 4;
    private int _currentPage = 1;

    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IApiServicesClient _apiServicesClient;
    private readonly ILocalSettingsService _localSettingsService;
    public ObservableCollection<Product> ProductLibraryItems { get; set; } = new ObservableCollection<Product>();
    public ObservableCollection<Product> FilteredProductLibraryItems { get; set; } = new ObservableCollection<Product>();

    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            SetProperty(ref _currentPage, value);
            UpdateFilteredProducts();
        }
    }

    public int TotalPages => (int)Math.Ceiling((double)ProductLibraryItems.Count / ItemsPerPage);

    private bool _canGoNext = true;
    public bool CanGoNext
    {
        get => _canGoNext;
        set => SetProperty(ref _canGoNext, value);
    }

    private bool _canGoBack = false;
    public bool CanGoBack
    {
        get => _canGoBack;
        set => SetProperty(ref _canGoBack, value);
    }
    public int CustomerId
    {
        get; set;
    }
    public int OrderId
    {
        get; set;
    }
    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }
    public ICommand SelectProductCommand { get; }
    public ICommand SelectCartCommand { get; }

    public ShopViewModel(INavigationService navigationService, IDialogService dialogService, IApiServicesClient apiServicesClient, ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _apiServicesClient = apiServicesClient;

        NextPageCommand = new RelayCommand(NextPage);
        PreviousPageCommand = new RelayCommand(PreviousPage);
        SelectCartCommand = new RelayCommand(SelectCart);
        SelectProductCommand = new AsyncRelayCommand<Product?>(SelectProduct);

        CreateOrGetOrderID();
        LoadProductLibraryAsync();
    }

    private void UpdateFilteredProducts()
    {
        FilteredProductLibraryItems.Clear();
        var items = ProductLibraryItems.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);
        foreach (var item in items)
        {
            FilteredProductLibraryItems.Add(item);
        }
    }

    private void NextPage()
    {
        if (CanGoNext)
        {
            CurrentPage++;
            CanGoBack = true;
            CanGoNext = (CurrentPage < TotalPages);
        }
    }


    private void PreviousPage()
    {
        if (CanGoBack)
        {
            CurrentPage--;
            CanGoNext = true;
            CanGoBack = (CurrentPage > 1);
        }
    }

    public async Task GetCustomerID()
    {
        var customer_id = await _localSettingsService.ReadSettingAsync<string>("customer_id");
        CustomerId = int.Parse(customer_id);
    }

    public async void CreateOrGetOrderID()
    {
        await GetCustomerID();
        var orderlist = _apiServicesClient.Get<List<Orders>>($"api/Order/get/customerID/All/" + CustomerId);
        var filteredOrder = orderlist.FirstOrDefault(order => string.IsNullOrEmpty(order.status));
        if (filteredOrder != null)
        {
            OrderId = filteredOrder.orders_id;
        }
        else
        {
            var order = new Orders
            {
                customer_id = CustomerId,
                order_date = DateTime.UtcNow,
                total_price = 0,
                address = "",
                phone_number = "",
                status = "",
                Orderdetail = new List<Orderdetail>()
            };
            var result = _apiServicesClient.Post<Orders>("api/Order/add", order);
            if(result == null)
            {
                _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: không thể tạo đơn hàng mới");
            }
        }
    }

    public async Task LoadProductLibraryAsync()
    {
        try
        {
            var products = _apiServicesClient.Get<List<Product>>("api/Product");
            if (products != null && products.Any())
            {
                ProductLibraryItems.Clear();
                foreach (var food in products)
                {
                    ProductLibraryItems.Add(food);
                }
            }
            UpdateFilteredProducts();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading product library: {ex.Message}");
            await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: " + ex.Message);
        }
    }

    public void SelectCart()
    {
        var pageKey = typeof(CartViewModel).FullName;
        if (pageKey != null)
        {
            _navigationService.NavigateTo(pageKey);
        }
    }

    public async Task SelectProduct(Product product)
    {
        var result = await _dialogService.ShowProductDialogWithSupplierAsync(product, OrderId);
        if(result == false)
        {
            await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: không thể thêm sản phẩm vào giỏ hàng");
        }
    }
}
