﻿// ViewModel cho trang cửa hàng.
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
using System.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;


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

    public ObservableCollection<Orderdetail> OrderDetailsItems { get; set; } = new ObservableCollection<Orderdetail>();

    private List<Product> _allProducts;

    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            SetProperty(ref _currentPage, value);
            UpdateFilteredAndSortedProducts();
        }
    }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value); 
            UpdateFilteredAndSortedProducts();
        }
    }

    private ComboBoxItem _selectedCategoryName = new() { Content = "Tất cả"};
    public ComboBoxItem SelectedCategoryName
    {
        get => _selectedCategoryName;
        set
        {
            SetProperty(ref _selectedCategoryName, value);
            UpdateFilteredAndSortedProducts();
        }
    }

    private ComboBoxItem _selectedSortOrder = new() { Content = "Gần đây"};
    public ComboBoxItem SelectedSortOrder
    {
        get => _selectedSortOrder;
        set
        {
            SetProperty(ref _selectedSortOrder, value); 
            UpdateFilteredAndSortedProducts();
        }
    }

    //public int TotalPages => (int)Math.Ceiling((double)ProductLibraryItems.Count / ItemsPerPage);
    private int _totalPages;
    public int TotalPages
    {
        get => _totalPages;
        set => SetProperty(ref _totalPages, value);
    }
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

    public ICommand SearchProductCommand { get; }


    private int _cartItemCount;
    public int CartItemCount
    {
        get => _cartItemCount;
        set => SetProperty(ref _cartItemCount, value);
    }
    

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
        SearchProductCommand = new RelayCommand(SearchProduct);
        
        CreateOrGetOrderID();
        LoadProductLibraryAsync();
        LoadOrderDetailData();
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

    
    public void SearchProduct()
    {
        UpdateFilteredAndSortedProducts();
    }

    private void UpdateFilteredAndSortedProducts()
    {
        IEnumerable<Product> filteredProducts = _allProducts;

        // Filter products based on the selected category
        if (SelectedCategoryName != null)
        {
            if (SelectedCategoryName.Content.ToString() != "Tất cả")
            {
                var categoryProducts = _apiServicesClient.Get<List<Product>>($"api/Product/get/category_name/" + SelectedCategoryName.Content.ToString());
                filteredProducts = (IEnumerable<Product>)categoryProducts;
            }
        }

        // Sort products based on the selected sort order
        if (SelectedSortOrder != null)
        {
            switch (SelectedSortOrder.Content)
            {
                case "Giá tăng dần":
                    filteredProducts = filteredProducts.OrderBy(p => p.product_price);
                    break;
                case "Giá giảm dần":
                    filteredProducts = filteredProducts.OrderByDescending(p => p.product_price);
                    break;
                case "Gần đây":
                    filteredProducts = filteredProducts.OrderBy(p => p.product_id); // Assuming product_id indicates recency
                    break;
            }
        }

        // Filter products based on the search text
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filteredProducts = filteredProducts.Where(p => p.product_name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        // Update the total pages based on the filtered products count
        int totalFilteredProducts = filteredProducts.Count();
        TotalPages = (int)Math.Ceiling((double)totalFilteredProducts / ItemsPerPage);

        // Ensure the current page is within the valid range
        if (CurrentPage > TotalPages)
        {
            CurrentPage = TotalPages;
        }
        else if (CurrentPage < 1)
        {
            CurrentPage = 1;
        }

        // Update the FilteredProductLibraryItems collection based on the current page
        FilteredProductLibraryItems.Clear();
        var pagedProducts = filteredProducts.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);
        foreach (var product in pagedProducts)
        {
            FilteredProductLibraryItems.Add(product);
        }

        // Update navigation properties
        CanGoNext = CurrentPage < TotalPages;
        CanGoBack = CurrentPage > 1;
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
                await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: không thể tạo đơn hàng mới");
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
                _allProducts = products;
                ProductLibraryItems.Clear();
                foreach (var food in products)
                {
                    ProductLibraryItems.Add(food);
                }
            }
            UpdateFilteredAndSortedProducts();
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
        if (result == false)
        {
            await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: không thể thêm sản phẩm vào giỏ hàng");
        }
        else
        {
            await _dialogService.ShowSuccessMessageAsync("Thêm sản phẩm vào giỏ hàng thành công");
            LoadOrderDetailData();
        }

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
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: " + ex.Message);
            Debug.WriteLine($"Error loading order details: {ex.Message}");
        }
        CartItemCount = OrderDetailsItems.Count;
    }
}
