namespace Gyminize.Contracts.ViewModels;

// Giao diện cho các ViewModel nhận biết điều hướng
// Giao diện này cung cấp các phương thức để xử lý sự kiện điều hướng đến và điều hướng đi từ một trang.
public interface INavigationAware
{
    // Phương thức được gọi khi điều hướng đến trang
    // Tham số parameter chứa dữ liệu được truyền khi điều hướng
    void OnNavigatedTo(object parameter);

    // Phương thức được gọi khi điều hướng đi từ trang
    void OnNavigatedFrom();
}