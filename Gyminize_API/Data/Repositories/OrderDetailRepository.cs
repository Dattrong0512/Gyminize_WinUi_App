using System.Runtime.InteropServices;
using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Gyminize_API.Data.Repositories;

/// <summary>
/// Lớp Repository để quản lý các thao tác dữ liệu chi tiết đơn hàng trong cơ sở dữ liệu.
/// </summary>
public class OrderDetailRepository
{
    private readonly EntityDatabaseContext _context;

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="OrderDetailRepository"/> với context cơ sở dữ liệu.
    /// </summary>
    /// <param name="context">Context cơ sở dữ liệu để tương tác với dữ liệu chi tiết đơn hàng.</param>
    public OrderDetailRepository(EntityDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Thêm mới hoặc cập nhật chi tiết đơn hàng.
    /// </summary>
    /// <param name="order_id">ID của đơn hàng.</param>
    /// <param name="number">Số lượng sản phẩm.</param>
    /// <param name="product">Sản phẩm liên quan đến chi tiết đơn hàng.</param>
    /// <returns>Chi tiết đơn hàng sau khi đã thêm hoặc cập nhật.</returns>
    public Orderdetail AddOrUpdateOrderdetail(int order_id, int number, Product product)
    {
        try
        {
            // Kiểm tra nếu OrderDetail đã tồn tại trong cơ sở dữ liệu
            var orderdetail = _context.OrderdetailEntity
                .Where(o => o.orders_id == order_id && o.product_id == product.product_id)
                .FirstOrDefault();

            if (orderdetail == null)
            {
                // Nếu không có OrderDetail, tạo mới
                orderdetail = new Orderdetail
                {
                    orders_id = order_id,
                    product_id = product.product_id,
                    product_amount = number
                };

                // Đảm bảo rằng product đã được "attach" vào ngữ cảnh
                _context.Attach(product); // Đính kèm đối tượng Product vào ngữ cảnh
                orderdetail.Product = product;

                // Thêm OrderDetail vào ngữ cảnh
                _context.OrderdetailEntity.Add(orderdetail);
            }
            else
            {
                // Nếu đã có OrderDetail, chỉ cần cập nhật số lượng
                orderdetail.product_amount += number;
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();
            return orderdetail;
        }
        catch (Exception ex)
        {
            // Log lỗi nếu có
            Console.WriteLine($"Lỗi trong AddOrUpdateOrderdetail: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Cập nhật số lượng sản phẩm của chi tiết đơn hàng.
    /// </summary>
    /// <param name="orderdetail_id">ID của chi tiết đơn hàng.</param>
    /// <param name="number_update">Số lượng sản phẩm mới.</param>
    /// <returns>Chi tiết đơn hàng đã được cập nhật.</returns>
    public Orderdetail OnlyUpdateOrderDetail(int orderdetail_id, int number_update)
    {
        try
        {
            var Orderdetail = _context.OrderdetailEntity
                .Where(o => o.orderdetail_id == orderdetail_id)
                .FirstOrDefault();
            if (Orderdetail != null)
            {
                Orderdetail.product_amount = number_update;
                _context.OrderdetailEntity.Update(Orderdetail);
                _context.SaveChanges();
            }
            return Orderdetail;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi trong OnlyUpdateOrderDetail: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Xóa sản phẩm khỏi chi tiết đơn hàng.
    /// </summary>
    /// <param name="orders_id">ID của đơn hàng.</param>
    /// <param name="product">Sản phẩm cần xóa khỏi chi tiết đơn hàng.</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy chi tiết đơn hàng để xóa.</returns>
    public bool DeleteFromOrderDetail(int orders_id, Product product)
    {
        var existingOrderDetail = _context.OrderdetailEntity
            .Where(o => o.orders_id == orders_id && o.product_id == product.product_id)
            .FirstOrDefault();

        if (existingOrderDetail != null)
        {
            _context.OrderdetailEntity.Remove(existingOrderDetail);
            _context.SaveChanges();
            return true;
        }
        return false;
    }
}
