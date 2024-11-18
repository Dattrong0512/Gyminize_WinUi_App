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

       
        public Customer_health UpdateWeightCustomer(int weight, int customerID )
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
}
