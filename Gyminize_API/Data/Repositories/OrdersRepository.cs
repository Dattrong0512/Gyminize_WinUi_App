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
            .Where(o => o.customer_id == customer_id) .ToList();
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
            try
            {
                _context.OrdersEntity.Add(order);
                _context.SaveChanges();
                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddOrder: {ex.Message}");
                throw;
            }
        }
    }
}


