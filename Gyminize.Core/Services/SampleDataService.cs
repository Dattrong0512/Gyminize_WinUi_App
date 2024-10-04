using Gyminize.Core.Contracts.Services;
using Gyminize.Core.Models;

namespace Gyminize.Core.Services;

// This class holds sample data used by some generated pages to show how they can be used.
// TODO: The following classes have been created to display sample data. Delete these files once your app is using real data.
// 1. Contracts/Services/ISampleDataService.cs
// 2. Services/SampleDataService.cs
// 3. Models/SampleCompany.cs
// 4. Models/SampleOrder.cs
// 5. Models/SampleOrderDetail.cs
public class SampleDataService : ISampleDataService
{
    private List<SampleOrder> _allOrders;

    public SampleDataService()
    {
    }

    private static IEnumerable<SampleOrder> AllOrders()
    {
        // Dữ liệu tóm tắt đơn hàng
        var companies = AllCompanies();
        return companies.SelectMany(c => c.Orders);
    }

    private static IEnumerable<SampleCompany> AllCompanies()
    {
        return new List<SampleCompany>()
        {
            new SampleCompany()
            {
                CompanyID = "ALFKI",
                CompanyName = "Company A",
                ContactName = "Maria Anders",
                ContactTitle = "Sales Representative",
                Address = "Obere Str. 57",
                City = "Berlin",
                PostalCode = "12209",
                Country = "Germany",
                Phone = "030-0074321",
                Fax = "030-0076545",
                Orders = new List<SampleOrder>()
                {
                    new SampleOrder()
                    {
                        OrderID = 10643, // Biểu tượng Globe
                        OrderDate = new DateTime(1997, 8, 25),
                        RequiredDate = new DateTime(1997, 9, 22),
                        ShippedDate = new DateTime(1997, 9, 22),
                        ShipperName = "Speedy Express",
                        ShipperPhone = "(503) 555-9831",
                        Freight = 29.46,
                        Company = "Company A",
                        ShipTo = "Company A, Obere Str. 57, Berlin, 12209, Germany",
                        OrderTotal = 814.50,
                        Status = "Shipped",
                        SymbolCode = 57643,
                        SymbolName = "Globe",
                        Details = new List<SampleOrderDetail>()
                        {
                            new SampleOrderDetail()
                            {
                                ProductID = 28,
                                ProductName = "Rössle Sauerkraut",
                                Quantity = 15,
                                Discount = 0.25,
                                QuantityPerUnit = "25 - 825 g cans",
                                UnitPrice = 45.60,
                                CategoryName = "Produce",
                                CategoryDescription = "Dried fruit and bean curd",
                                Total = 513.00
                            },
                            new SampleOrderDetail()
                            {
                                ProductID = 39,
                                ProductName = "Chartreuse verte",
                                Quantity = 21,
                                Discount = 0.25,
                                QuantityPerUnit = "750 cc per bottle",
                                UnitPrice = 18.0,
                                CategoryName = "Beverages",
                                CategoryDescription = "Soft drinks, coffees, teas, beers, and ales",
                                Total = 283.50
                            },
                            new SampleOrderDetail()
                            {
                                ProductID = 46,
                                ProductName = "Spegesild",
                                Quantity = 2,
                                Discount = 0.25,
                                QuantityPerUnit = "4 - 450 g glasses",
                                UnitPrice = 12.0,
                                CategoryName = "Seafood",
                                CategoryDescription = "Seaweed and fish",
                                Total = 18.00
                            }
                        }
                    },
                    new SampleOrder()
                    {
                        OrderID = 10835, // Biểu tượng Music
                        OrderDate = new DateTime(1998, 1, 15),
                        RequiredDate = new DateTime(1998, 2, 12),
                        ShippedDate = new DateTime(1998, 1, 21),
                        ShipperName = "Federal Shipping",
                        ShipperPhone = "(503) 555-9931",
                        Freight = 69.53,
                        Company = "Company A",
                        ShipTo = "Company A, Obere Str. 57, Berlin, 12209, Germany",
                        OrderTotal = 845.80,
                        Status = "Closed",
                        SymbolCode = 57737,
                        SymbolName = "Audio",
                        Details = new List<SampleOrderDetail>()
                        {
                            new SampleOrderDetail()
                            {
                                ProductID = 59,
                                ProductName = "Raclette Courdavault",
                                Quantity = 15,
                                Discount = 0,
                                QuantityPerUnit = "5 kg pkg.",
                                UnitPrice = 55.00,
                                CategoryName = "Dairy Products",
                                CategoryDescription = "Cheeses",
                                Total = 825.00
                            },
                            new SampleOrderDetail()
                            {
                                ProductID = 77,
                                ProductName = "Original Frankfurter grüne Soße",
                                Quantity = 2,
                                Discount = 0.2,
                                QuantityPerUnit = "12 boxes",
                                UnitPrice = 13.0,
                                CategoryName = "Condiments",
                                CategoryDescription = "Sweet and savory sauces, relishes, spreads, and seasonings",
                                Total = 20.80
                            }
                        }
                    },
                    new SampleOrder()
                    {
                        OrderID = 10952, // Biểu tượng Calendar
                        OrderDate = new DateTime(1998, 3, 16),
                        RequiredDate = new DateTime(1998, 4, 27),
                        ShippedDate = new DateTime(1998, 3, 24),
                        ShipperName = "Speedy Express",
                        ShipperPhone = "(503) 555-9831",
                        Freight = 40.42,
                        Company = "Company A",
                        ShipTo = "Company A, Obere Str. 57, Berlin, 12209, Germany",
                        OrderTotal = 471.20,
                        Status = "Closed",
                        SymbolCode = 57699,
                        SymbolName = "Calendar",
                        Details = new List<SampleOrderDetail>()
                        {
                            new SampleOrderDetail()
                            {
                                ProductID = 6,
                                ProductName = "Grandma's Boysenberry Spread",
                                Quantity = 16,
                                Discount = 0.05,
                                QuantityPerUnit = "12 - 8 oz jars",
                                UnitPrice = 25.0,
                                CategoryName = "Condiments",
                                CategoryDescription = "Sweet and savory sauces, relishes, spreads, and seasonings",
                                Total = 380.00
                            },
                            new SampleOrderDetail()
                            {
                                ProductID = 28,
                                ProductName = "Rössle Sauerkraut",
                                Quantity = 2,
                                Discount = 0,
                                QuantityPerUnit = "25 - 825 g cans",
                                UnitPrice = 45.60,
                                CategoryName = "Produce",
                                CategoryDescription = "Dried fruit and bean curd",
                                Total = 91.20
                            }
                        }
                    }
                }
            }
        };
    }

    public async Task<IEnumerable<SampleOrder>> GetContentGridDataAsync()
    {
        // Kiểm tra nếu _allOrders là null, nếu đúng thì khởi tạo _allOrders với danh sách các đơn hàng từ phương thức AllOrders().
        _allOrders ??= new List<SampleOrder>(AllOrders());

        // Hoàn thành tác vụ bất đồng bộ ngay lập tức.
        await Task.CompletedTask;

        // Trả về danh sách các đơn hàng.
        return _allOrders;
    }
}
