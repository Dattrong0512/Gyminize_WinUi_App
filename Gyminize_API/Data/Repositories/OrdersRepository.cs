using System.Diagnostics;
using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data.Repositories
{
    public class OrdersRepository
    {
        private readonly EntityDatabaseContext _context;
        public OrdersRepository(EntityDatabaseContext context)
        {
            _context = context;
        }
        public List<Orders>? GetAllOrderByCustomerId(int customer_id)
        {
            var Orders = _context.OrdersEntity
                .Include(o => o.Orderdetail)
                  .ThenInclude(p=>p.Product)
            .Where(o => o.customer_id == customer_id).ToList();
            return Orders;

        }
        public Orders? GetOrderByCustomerId(int customer_id)
        {
            var Orders = _context.OrdersEntity
                .Include(o => o.Orderdetail)
                  .ThenInclude(p => p.Product)
            .Where(o => o.customer_id == customer_id).FirstOrDefault();
            return Orders;

        }
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
                        throw new Exception($"Product with ID {detail.product_id} not found.");
                    }
                }
            }
            Debug.WriteLine(order);
            // Thêm đơn hàng vào database
            _context.OrdersEntity.Add(order);
            _context.SaveChanges();

            return order;
        }

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
                Console.WriteLine($"Error in UpdateStatusOrder: {ex.Message}");
                throw;
            }
        }
    }
}


