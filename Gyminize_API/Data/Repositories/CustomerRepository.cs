using Gyminize_API.Data.Models;

namespace Gyminize_API.Data.Repositories
{
    public class CustomerRepository
    {
        private readonly EntityDatabaseContext _context;
        public CustomerRepository(EntityDatabaseContext context)
        {
            _context = context;
        }
        public List<Customer> GetAllCustomer()
        {
            try
            {
                return _context.CustomerEntity.ToList();
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                Console.WriteLine($"Error in GetAllCustomer: {ex.Message}");
                throw;
            }
            
        }

        public Customer? GetCustomerById(int id)
        {
            return _context.CustomerEntity.Where(x => x.customer_id == id).FirstOrDefault();
        }
        public Customer? GetCustomerByUsername(string username)
        {
            return _context.CustomerEntity.Where(x => x.username == username).FirstOrDefault();
        }
        public Customer addCustomer(Customer customer)
        {
            _context.CustomerEntity.Add(customer);
            _context.SaveChanges();
            return customer;
        }
        public Customer updateCustomer(string username, Customer customer)
        {
            var check_customer = _context.CustomerEntity.Where(x => x.username == customer.username).FirstOrDefault();
            if (check_customer != null)
            {
                check_customer.username = customer.username;
                check_customer.customer_password = customer.customer_password;
                check_customer.auth_type = customer.auth_type;
                check_customer.customer_name = customer.customer_name;
                return customer;
            }
            _context.SaveChanges();
            return check_customer;
        }
        public void DeleteCustomer(Customer customer)
        {
            var check_customer = _context.CustomerEntity.Where(x => x.username == customer.username).FirstOrDefault();
            if (check_customer != null)
            {
                _context.CustomerEntity.Remove(customer);
                _context.SaveChanges();
            }
        }
    }
}
