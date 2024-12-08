using Gyminize_API.Data.Models;

namespace Gyminize_API.Data.Repositories;

/// <summary>
/// Lớp Repository để quản lý các thao tác dữ liệu khách hàng trong cơ sở dữ liệu.
/// </summary>
public class CustomerRepository
{
    private readonly EntityDatabaseContext _context;

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="CustomerRepository"/>.
    /// </summary>
    /// <param name="context">Context cơ sở dữ liệu để tương tác với dữ liệu khách hàng.</param>
    public CustomerRepository(EntityDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy khách hàng từ cơ sở dữ liệu dựa trên ID khách hàng duy nhất.
    /// </summary>
    /// <param name="id">Mã ID duy nhất của khách hàng.</param>
    /// <returns>Khách hàng nếu tìm thấy, nếu không trả về null.</returns>
    public Customer? GetCustomerById(int id)
    {
        return _context.CustomerEntity.Where(x => x.customer_id == id).FirstOrDefault();
    }

    /// <summary>
    /// Lấy khách hàng từ cơ sở dữ liệu dựa trên tên người dùng của họ.
    /// </summary>
    /// <param name="username">Tên người dùng của khách hàng.</param>
    /// <returns>Khách hàng nếu tìm thấy, nếu không trả về null.</returns>
    public Customer? GetCustomerByUsername(string username)
    {
        return _context.CustomerEntity.Where(x => x.username == username).FirstOrDefault();
    }

    /// <summary>
    /// Lấy khách hàng từ cơ sở dữ liệu dựa trên địa chỉ email của họ.
    /// </summary>
    /// <param name="email">Địa chỉ email của khách hàng.</param>
    /// <returns>Khách hàng nếu tìm thấy, nếu không trả về null.</returns>
    public Customer? GetCustomerByEmail(string email)
    {
        return _context.CustomerEntity.Where(x => x.email == email).FirstOrDefault();
    }

    /// <summary>
    /// Thêm một khách hàng mới vào cơ sở dữ liệu.
    /// </summary>
    /// <param name="customer">Dữ liệu khách hàng cần thêm.</param>
    /// <returns>Dữ liệu khách hàng đã được thêm.</returns>
    /// <exception cref="Exception">Ném ra ngoại lệ nếu đã tồn tại một khách hàng với cùng ID.</exception>
    public Customer AddCustomer(Customer customer)
    {
        var existingCustomer = _context.CustomerEntity.Find(customer.customer_id);
        if (existingCustomer != null)
        {
            throw new Exception("Khách hàng với ID này đã tồn tại.");
        }
        _context.CustomerEntity.Add(customer);
        _context.SaveChanges();
        return customer;
    }

    /// <summary>
    /// Cập nhật mật khẩu của khách hàng dựa trên tên người dùng.
    /// </summary>
    /// <param name="username">Tên người dùng của khách hàng cần cập nhật mật khẩu.</param>
    /// <param name="password">Mật khẩu mới của khách hàng.</param>
    /// <returns>Khách hàng đã được cập nhật mật khẩu mới.</returns>
    public Customer UpdatePasswordByUser(string username, string password)
    {
        var checkCustomer = _context.CustomerEntity.Where(x => x.username == username).FirstOrDefault();
        if (checkCustomer != null)
        {
            checkCustomer.customer_password = password;
            _context.SaveChanges();
            return checkCustomer;
        }
        return checkCustomer;
    }

    /// <summary>
    /// Cập nhật thông tin khách hàng.
    /// </summary>
    /// <param name="username">Tên người dùng của khách hàng cần cập nhật thông tin.</param>
    /// <param name="customer">Dữ liệu khách hàng mới để cập nhật.</param>
    /// <returns>Dữ liệu khách hàng đã được cập nhật.</returns>
    public Customer UpdateCustomer(string username, Customer customer)
    {
        var checkCustomer = _context.CustomerEntity.Where(x => x.username == customer.username).FirstOrDefault();
        if (checkCustomer != null)
        {
            checkCustomer.username = customer.username;
            checkCustomer.customer_password = customer.customer_password;
            checkCustomer.auth_type = customer.auth_type;
            checkCustomer.customer_name = customer.customer_name;
            checkCustomer.role_user = customer.role_user;
            checkCustomer.email = customer.email;
            _context.SaveChanges();
            return checkCustomer;
        }
        return null;  // Trả về null nếu không tìm thấy khách hàng
    }
}
