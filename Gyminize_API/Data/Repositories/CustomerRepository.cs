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
            var existingCustomer = _context.CustomerEntity.Find(customer.customer_id);
            if (existingCustomer != null)
            {
                throw new Exception("Customer with the same ID already exists.");
            }
            _context.CustomerEntity.Add(customer);
            _context.SaveChanges();
            return customer;
        }
        public Customer updatePassworkByUser(string username, string password)
        {
            var check_customer = _context.CustomerEntity.Where(x=>x.username == username).FirstOrDefault();
            if(check_customer != null)
            {
                check_customer.customer_password = password;
                _context.SaveChanges();
                return check_customer;
            }
            return check_customer;
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
                check_customer.auth_type = customer.auth_type;
                check_customer.role_user = customer.role_user;
                check_customer.email = customer.email;
                return customer;
            }
            _context.SaveChanges();
            return check_customer;
        }
    }
}
