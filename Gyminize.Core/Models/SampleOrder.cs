// Mô hình cho SampleDataService. Thay thế bằng mô hình của bạn.
namespace Gyminize.Core.Models;

public class SampleOrder
{
    // ID của đơn hàng.
    public long OrderID
    {
        get; set;
    }

    // Ngày đặt hàng.
    public DateTime OrderDate
    {
        get; set;
    }

    // Ngày yêu cầu giao hàng.
    public DateTime RequiredDate
    {
        get; set;
    }

    // Ngày giao hàng.
    public DateTime ShippedDate
    {
        get; set;
    }

    // Tên người giao hàng.
    public string ShipperName
    {
        get; set;
    }

    // Số điện thoại người giao hàng.
    public string ShipperPhone
    {
        get; set;
    }

    // Phí vận chuyển.
    public double Freight
    {
        get; set;
    }

    // Tên công ty đặt hàng.
    public string Company
    {
        get; set;
    }

    // Địa chỉ giao hàng.
    public string ShipTo
    {
        get; set;
    }

    // Tổng giá trị đơn hàng.
    public double OrderTotal
    {
        get; set;
    }

    // Trạng thái đơn hàng.
    public string Status
    {
        get; set;
    }

    // Mã biểu tượng.
    public int SymbolCode
    {
        get; set;
    }

    // Tên biểu tượng.
    public string SymbolName
    {
        get; set;
    }

    // Biểu tượng.
    public char Symbol => (char)SymbolCode;

    // Danh sách chi tiết đơn hàng.
    public ICollection<SampleOrderDetail> Details
    {
        get; set;
    }

    // Mô tả ngắn gọn của đơn hàng.
    public string ShortDescription => $"Order ID: {OrderID}";

    // Phương thức chuyển đổi đối tượng thành chuỗi.
    public override string ToString() => $"{Company} {Status}";
}