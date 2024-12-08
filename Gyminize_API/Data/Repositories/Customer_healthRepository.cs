using Gyminize_API.Data.Model;

namespace Gyminize_API.Data.Repositories;

/// <summary>
/// Repository để quản lý dữ liệu sức khỏe của khách hàng trong cơ sở dữ liệu.
/// </summary>
public class CustomerHealthRepository
{
    private readonly EntityDatabaseContext _context;

    /// <summary>
    /// Khởi tạo một instance mới của <see cref="CustomerHealthRepository"/> với context cơ sở dữ liệu đã cho.
    /// </summary>
    /// <param name="context">Context cơ sở dữ liệu để tương tác với dữ liệu sức khỏe của khách hàng.</param>
    public CustomerHealthRepository(EntityDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy dữ liệu sức khỏe của khách hàng theo ID của họ.
    /// </summary>
    /// <param name="customerId">Mã ID duy nhất của khách hàng.</param>
    /// <returns>Dữ liệu sức khỏe của khách hàng nếu tìm thấy, nếu không trả về null.</returns>
    public Customer_health? GetCustomerHealthByCustomerId(int customerId)
    {
        return _context.CustomerHealthEntity.FirstOrDefault(ch => ch.customer_id == customerId);
    }

    /// <summary>
    /// Thêm một bản ghi sức khỏe khách hàng mới vào cơ sở dữ liệu.
    /// </summary>
    /// <param name="customerHealth">Dữ liệu sức khỏe của khách hàng cần thêm.</param>
    /// <returns>Bản ghi sức khỏe khách hàng đã được thêm.</returns>
    /// <exception cref="Exception">Ném ra ngoại lệ nếu có lỗi trong quá trình thực hiện.</exception>
    public Customer_health AddCustomerHealth(Customer_health customerHealth)
    {
        try
        {
            _context.CustomerHealthEntity.Add(customerHealth);
            _context.SaveChanges();
            return customerHealth;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi trong AddCustomerHealth: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Cập nhật cân nặng của khách hàng trong bản ghi sức khỏe theo ID khách hàng.
    /// </summary>
    /// <param name="weight">Cân nặng mới cần cập nhật cho khách hàng.</param>
    /// <param name="customerID">Mã ID duy nhất của khách hàng.</param>
    /// <returns>Dữ liệu sức khỏe của khách hàng đã được cập nhật.</returns>
    public Customer_health UpdateWeightCustomer(int weight, int customerID)
    {
        var check_customerHealth = _context.CustomerHealthEntity.FirstOrDefault(ch => ch.customer_id == customerID);

        if (check_customerHealth != null)
        {
            check_customerHealth.weight = weight;
            _context.CustomerHealthEntity.Update(check_customerHealth);
        }
        _context.SaveChanges();
        return check_customerHealth;
    }
}
