using Gyminize_API.Data.Model;

namespace Gyminize_API.Data.Repositories
{
    public class PaymentRepository
    {
        private readonly EntityDatabaseContext _context;
        public PaymentRepository(EntityDatabaseContext context)
        {
            _context = context;
        }
        public Payment? GetPaymentByOrderID(int orderID)
        {
            return _context.PaymentEntity.FirstOrDefault(p => p.orders_id == orderID);
        }
        public Payment AddPayment(Payment payment)
        {
            try
            {
                _context.PaymentEntity.Add(payment);
                _context.SaveChanges();
                return payment;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddPayment: {ex.Message}");
                throw;
            }
        }
    }
}


