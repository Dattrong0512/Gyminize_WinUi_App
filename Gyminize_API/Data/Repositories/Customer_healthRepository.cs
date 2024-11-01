using Gyminize_API.Data.Model;

namespace Gyminize_API.Data.Repositories
{
    public class CustomerHealthRepository
    {
        private readonly EntityDatabaseContext _context;

        public CustomerHealthRepository(EntityDatabaseContext context)
        {
            _context = context;
        }

        public List<Customer_health> GetAllCustomerHealth()
        {
            try
            {
                return _context.CustomerHealthEntity.ToList();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error in GetAllCustomerHealth: {ex.Message}");
                throw;
            }
        }

        public Customer_health? GetCustomerHealthByCustomerId(int customerId)
        {
            return _context.CustomerHealthEntity.FirstOrDefault(ch => ch.customer_id == customerId);
        }

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
                Console.WriteLine($"Error in AddCustomerHealth: {ex.Message}");
                throw;
            }


        }

        public Customer_health UpdateCustomerHealth(Customer_health customerHealth)
        {
            var check_customerHealth = _context.CustomerHealthEntity.FirstOrDefault(ch => ch.customer_id == customerHealth.customer_id);
            if (check_customerHealth != null)
            {
                _context.CustomerHealthEntity.Update(customerHealth);
            }
            _context.SaveChanges();
            return check_customerHealth;

        }

        public void DeleteCustomerHealth(int customerId)
        {
            var customerHealth = _context.CustomerHealthEntity.FirstOrDefault(ch => ch.customer_id == customerId);
            if (customerHealth != null)
            {
                _context.CustomerHealthEntity.Remove(customerHealth);
                _context.SaveChanges();
            }
        }
    }
}
