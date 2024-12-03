using System.Runtime.InteropServices;
using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data.Repositories
{
    public class OrderDetailRepository
    {
        private readonly EntityDatabaseContext _context;
        public OrderDetailRepository(EntityDatabaseContext context)
        {
            _context = context;
        }
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
                Console.WriteLine($"Error in AddOrUpdateOrderdetail: {ex.Message}");
                throw;
            }
        }

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
                Console.WriteLine($"Error in OnlyUpdateOrderDetail: {ex.Message}");
                throw;
            }
        }

        public bool DeleteFromOrderDetail(int orders_id, Product product)
        {
            var existingOrderDetail = _context.OrderdetailEntity
                .Where(o => o.orders_id == orders_id && o.product_id == product.product_id)
                .FirstOrDefault();

            if(existingOrderDetail != null)
            {
                _context.OrderdetailEntity.Remove(existingOrderDetail);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}


