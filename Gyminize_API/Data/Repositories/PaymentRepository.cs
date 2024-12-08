using Gyminize_API.Data.Model;

namespace Gyminize_API.Data.Repositories;

/// <summary>
/// Lớp Repository để quản lý các thao tác dữ liệu thanh toán trong cơ sở dữ liệu.
/// </summary>
public class PaymentRepository
{
    private readonly EntityDatabaseContext _context;

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="PaymentRepository"/> với context cơ sở dữ liệu.
    /// </summary>
    /// <param name="context">Context cơ sở dữ liệu để tương tác với dữ liệu thanh toán.</param>
    public PaymentRepository(EntityDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy thông tin thanh toán dựa trên orderID.
    /// </summary>
    /// <param name="orderID">ID của đơn hàng cần tìm thông tin thanh toán.</param>
    /// <returns>Thông tin thanh toán của đơn hàng nếu có, ngược lại trả về null.</returns>
    public Payment? GetPaymentByOrderID(int orderID)
    {
        return _context.PaymentEntity.FirstOrDefault(p => p.orders_id == orderID);
    }

    /// <summary>
    /// Thêm thông tin thanh toán mới vào cơ sở dữ liệu.
    /// </summary>
    /// <param name="payment">Thông tin thanh toán cần thêm.</param>
    /// <returns>Thông tin thanh toán đã được thêm vào cơ sở dữ liệu.</returns>
    public Payment AddPayment(Payment payment)
    {
        try
        {
            // Thêm thông tin thanh toán vào cơ sở dữ liệu
            _context.PaymentEntity.Add(payment);
            _context.SaveChanges();
            return payment;
        }
        catch (Exception ex)
        {
            // Ghi log nếu có lỗi
            Console.WriteLine($"Lỗi trong AddPayment: {ex.Message}");
            throw;
        }
    }
}
