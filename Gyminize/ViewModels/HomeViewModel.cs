// ViewModel cho trang chủ.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gyminize.ViewModels;

public partial class HomeViewModel : ObservableRecipient
{
    // Khởi tạo HomeViewModel.
    public HomeViewModel()
    {
    }
}