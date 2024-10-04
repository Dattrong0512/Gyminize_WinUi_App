// ViewModel cho trang dinh dưỡng.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gyminize.ViewModels;

public partial class NutritionsViewModel : ObservableRecipient
{
    // Khởi tạo NutritionsViewModel.
    public NutritionsViewModel()
    {
    }
}

