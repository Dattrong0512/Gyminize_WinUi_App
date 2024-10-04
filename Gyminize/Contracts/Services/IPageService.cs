namespace Gyminize.Contracts.Services;

// Giao diện cho dịch vụ trang
// Dịch vụ này chịu trách nhiệm lấy loại trang dựa trên khóa trang.
public interface IPageService
{
    // Lấy loại trang dựa trên khóa trang
    Type GetPageType(string key);
}
