using System.Diagnostics;
using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Gyminize_API.Data.Repositories;

/// <summary>
/// Lớp Repository để quản lý các thao tác dữ liệu đơn hàng trong cơ sở dữ liệu.
/// </summary>
public class OrdersRepository
{
    private readonly EntityDatabaseContext _context;

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="OrdersRepository"/> với context cơ sở dữ liệu.
    /// </summary>
    /// <param name="context">Context cơ sở dữ liệu để tương tác với dữ liệu đơn hàng.</param>
    public OrdersRepository(EntityDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy tất cả đơn hàng của một khách hàng dựa trên customer_id.
    /// </summary>
    /// <param name="customer_id">ID của khách hàng.</param>
    /// <returns>Danh sách đơn hàng của khách hàng.</returns>
    public List<Orders>? GetAllOrderByCustomerId(int customer_id)
    {
        var Orders = _context.OrdersEntity
            .Include(o => o.Orderdetail)
              .ThenInclude(p => p.Product)
            .Where(o => o.customer_id == customer_id).ToList();
        return Orders;
    }

    /// <summary>
    /// Lấy đơn hàng đầu tiên của một khách hàng dựa trên customer_id.
    /// </summary>
    /// <param name="customer_id">ID của khách hàng.</param>
    /// <returns>Đơn hàng đầu tiên của khách hàng nếu có.</returns>
    public Orders? GetOrderByCustomerId(int customer_id)
    {
        var Orders = _context.OrdersEntity
            .Include(o => o.Orderdetail)
              .ThenInclude(p => p.Product)
            .Where(o => o.customer_id == customer_id).FirstOrDefault();
        return Orders;
    }

    /// <summary>
    /// Thêm một đơn hàng mới vào cơ sở dữ liệu.
    /// </summary>
    /// <param name="order">Đơn hàng cần thêm.</param>
    /// <returns>Đơn hàng đã được thêm vào cơ sở dữ liệu.</returns>
    public Orders AddOrder(Orders order)
    {
        // Kiểm tra xem customer_id có hợp lệ không

        // Nếu đơn hàng có chi tiết, thêm vào Orderdetail
        if (order.Orderdetail != null && order.Orderdetail.Any())
        {
            foreach (var detail in order.Orderdetail)
            {
                var product = _context.ProductEntity.Find(detail.product_id);
                if (product == null)
                {
                    throw new Exception($"Sản phẩm với ID {detail.product_id} không tìm thấy.");
                }
            }
        }
        Debug.WriteLine(order);
        // Thêm đơn hàng vào database
        _context.OrdersEntity.Add(order);
        _context.SaveChanges();

        return order;
    }

    /// <summary>
    /// Cập nhật trạng thái đơn hàng.
    /// </summary>
    /// <param name="orders_id">ID của đơn hàng cần cập nhật trạng thái.</param>
    /// <param name="status">Trạng thái mới của đơn hàng.</param>
    /// <returns>True nếu cập nhật thành công, False nếu không tìm thấy đơn hàng.</returns>
    public bool UpdateStatusOrder(int orders_id, string status)
    {
        try
        {
            var order = _context.OrdersEntity
                .Where(o => o.orders_id == orders_id)
                .FirstOrDefault();
            if (order == null)
            {
                return false;
            }
            order.status = status;
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi trong UpdateStatusOrder: {ex.Message}");
            throw;
        }
    }
}
