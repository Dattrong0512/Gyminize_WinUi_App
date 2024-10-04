// Mô hình cho SampleDataService. Thay thế bằng mô hình của bạn.
namespace Gyminize.Core.Models;

public class SampleOrderDetail
{
    // ID của sản phẩm.
    public long ProductID
    {
        get; set;
    }

    // Tên sản phẩm.
    public string ProductName
    {
        get; set;
    }

    // Số lượng sản phẩm.
    public int Quantity
    {
        get; set;
    }

    // Giảm giá.
    public double Discount
    {
        get; set;
    }

    // Số lượng mỗi đơn vị.
    public string QuantityPerUnit
    {
        get; set;
    }

    // Giá mỗi đơn vị.
    public double UnitPrice
    {
        get; set;
    }

    // Tên danh mục.
    public string CategoryName
    {
        get; set;
    }

    // Mô tả danh mục.
    public string CategoryDescription
    {
        get; set;
    }

    // Tổng giá trị.
    public double Total
    {
        get; set;
    }

    // Mô tả ngắn gọn của sản phẩm.
    public string ShortDescription => $"Product ID: {ProductID} - {ProductName}";
}