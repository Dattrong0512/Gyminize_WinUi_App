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


namespace Gyminize.ViewModels;

public partial class ShopViewModel : ObservableRecipient
{
    private const int ItemsPerPage = 4;
    private int _currentPage = 1;

    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IApiServicesClient _apiServicesClient;
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

    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }
    public ICommand SelectProductCommand { get; }
    public ICommand SelectCartCommand { get; }

    public ShopViewModel(INavigationService navigationService, IDialogService dialogService, IApiServicesClient apiServicesClient)
    {
        _navigationService = navigationService;
        _dialogService = dialogService;
        _apiServicesClient = apiServicesClient;

        NextPageCommand = new RelayCommand(NextPage);
        PreviousPageCommand = new RelayCommand(PreviousPage);
        SelectCartCommand = new RelayCommand(SelectCart);
        SelectProductCommand = new AsyncRelayCommand<Product?>(SelectProduct);
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


    public async Task LoadProductLibraryAsync()
    {
        try
        {
            var foods = _apiServicesClient.Get<List<Product>>("api/Product");
            if (foods != null && foods.Any())
            {
                ProductLibraryItems.Clear();
                foreach (var food in foods)
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
        await _dialogService.ShowProductDialogWithSupplierAsync(product);
    }
}
