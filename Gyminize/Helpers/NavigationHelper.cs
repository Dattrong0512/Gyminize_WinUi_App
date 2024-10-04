using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Helpers;

// Lớp trợ giúp để thiết lập mục tiêu điều hướng cho NavigationViewItem.
//
// Sử dụng trong XAML:
// <NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavigationHelper.NavigateTo="AppName.ViewModels.MainViewModel" />
//
// Sử dụng trong mã:
// NavigationHelper.SetNavigateTo(navigationViewItem, typeof(MainViewModel).FullName);
public class NavigationHelper
{
    // Lấy giá trị NavigateTo từ NavigationViewItem
    public static string GetNavigateTo(NavigationViewItem item) => (string)item.GetValue(NavigateToProperty);

    // Thiết lập giá trị NavigateTo cho NavigationViewItem
    public static void SetNavigateTo(NavigationViewItem item, string value) => item.SetValue(NavigateToProperty, value);

    // Thuộc tính phụ thuộc NavigateTo
    public static readonly DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationHelper), new PropertyMetadata(null));
}