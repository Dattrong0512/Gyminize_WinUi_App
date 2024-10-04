// Mô hình cho SampleDataService. Thay thế bằng mô hình của bạn.
namespace Gyminize.Core.Models;

public class SampleCompany
{
    // ID của công ty.
    public string CompanyID
    {
        get; set;
    }

    // Tên của công ty.
    public string CompanyName
    {
        get; set;
    }

    // Tên người liên hệ.
    public string ContactName
    {
        get; set;
    }

    // Chức danh của người liên hệ.
    public string ContactTitle
    {
        get; set;
    }

    // Địa chỉ của công ty.
    public string Address
    {
        get; set;
    }

    // Thành phố của công ty.
    public string City
    {
        get; set;
    }

    // Mã bưu điện của công ty.
    public string PostalCode
    {
        get; set;
    }

    // Quốc gia của công ty.
    public string Country
    {
        get; set;
    }

    // Số điện thoại của công ty.
    public string Phone
    {
        get; set;
    }

    // Số fax của công ty.
    public string Fax
    {
        get; set;
    }

    // Danh sách các đơn hàng của công ty.
    public ICollection<SampleOrder> Orders
    {
        get; set;
    }
}