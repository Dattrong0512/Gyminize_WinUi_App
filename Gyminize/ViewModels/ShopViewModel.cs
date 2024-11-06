//// ViewModel cho trang cửa hàng.
//// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
//// Thực hiện giao diện INavigationAware để nhận biết điều hướng.
//using System.Collections.ObjectModel;
//using System.Windows.Input;

//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;

//using Gyminize.Contracts.Services;
//using Gyminize.Contracts.ViewModels;
//using Gyminize.Core.Contracts.Services;
//using Gyminize.Core.Models;

//namespace Gyminize.ViewModels;

//public partial class ShopViewModel : ObservableRecipient, INavigationAware
//{
//    // Dịch vụ điều hướng.
//    private readonly INavigationService _navigationService;
//    // Dịch vụ dữ liệu mẫu.
//    private readonly ISampleDataService _sampleDataService;

//    // Thuộc tính lưu trữ danh sách các đơn hàng mẫu.
//    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

//    // Khởi tạo ShopViewModel với INavigationService và ISampleDataService.
//    public ShopViewModel(INavigationService navigationService, ISampleDataService sampleDataService)
//    {
//        _navigationService = navigationService;
//        _sampleDataService = sampleDataService;
//    }

//    // Phương thức được gọi khi điều hướng đến trang.
//    public async void OnNavigatedTo(object parameter)
//    {
//        Source.Clear();

//        // TODO: Thay thế bằng dữ liệu thực tế.
//        var data = await _sampleDataService.GetContentGridDataAsync();
//        foreach (var item in data)
//        {
//            Source.Add(item);
//        }
//    }

//    // Phương thức được gọi khi điều hướng đi từ trang.
//    public void OnNavigatedFrom()
//    {
//    }

//    // Lệnh xử lý sự kiện khi một mục được nhấp vào.
//    [RelayCommand]
//    private void OnItemClick(SampleOrder? clickedItem)
//    {
//        if (clickedItem != null)
//        {
//            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
//            _navigationService.NavigateTo(typeof(ShopDetailViewModel).FullName!, clickedItem.OrderID);
//        }
//    }
//}