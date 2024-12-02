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

public partial class ShopViewModel : ObservableRecipient, INavigationAware
{
    // Dịch vụ điều hướng.
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IApiServicesClient _apiServicesClient;
    public ObservableCollection<Product> ProductLibraryItems { get; set; } = new ObservableCollection<Product>();
    public ObservableCollection<Product> FilteredProductLibraryItems { get; set; } = new ObservableCollection<Product>();
    public ShopViewModel(INavigationService navigationService, IDialogService dialogService, IApiServicesClient apiServicesClient)
    {
        _navigationService = navigationService;
        _dialogService = dialogService;
        _apiServicesClient = apiServicesClient;

        LoadProductLibraryAsync();
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
            FilteredProductLibraryItems = new ObservableCollection<Product>(ProductLibraryItems);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading product library: {ex.Message}");
            await _dialogService.ShowErrorDialogAsync("Lỗi hệ thống: " + ex.Message);
        }
    }


    // Phương thức được gọi khi điều hướng đến trang.
    public async void OnNavigatedTo(object parameter)
    {
        
    }

    // Phương thức được gọi khi điều hướng đi từ trang.
    public void OnNavigatedFrom()
    {
    }

}