// ViewModel cho trang chi tiết cửa hàng.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
// Thực hiện giao diện INavigationAware để nhận biết điều hướng.
using CommunityToolkit.Mvvm.ComponentModel;

using Gyminize.Contracts.ViewModels;
using Gyminize.Core.Contracts.Services;
using Gyminize.Core.Models;

namespace Gyminize.ViewModels;

public partial class ShopDetailViewModel : ObservableRecipient, INavigationAware
{
    // Dịch vụ dữ liệu mẫu.
    private readonly ISampleDataService _sampleDataService;

    // Thuộc tính lưu trữ mục được chọn.
    [ObservableProperty]
    private SampleOrder? item;

    // Khởi tạo ShopDetailViewModel với ISampleDataService.
    public ShopDetailViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    // Phương thức được gọi khi điều hướng đến trang.
    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is long orderID)
        {
            var data = await _sampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.OrderID == orderID);
        }
    }

    // Phương thức được gọi khi điều hướng đi từ trang.
    public void OnNavigatedFrom()
    {
    }
}