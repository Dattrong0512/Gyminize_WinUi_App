using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
// Import các thành phần của ASP.NET Core MVC.
// ControllerBase và các thuộc tính như [HttpGet], [HttpPost], [HttpPut], [HttpDelete]
// đều thuộc về thư viện này.
namespace Gyminize_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(CustomerRepository customerRepository) : ControllerBase
    {
        private readonly CustomerRepository _customerRepository = customerRepository;
        [HttpGet("get/username/{username}")]
        public IActionResult GetCustomerByUserName(string username)
        {
            var customer = _customerRepository.GetCustomerByUsername(username);
            return Ok(customer);
        }

        [HttpGet("get/{id:int}")]
        public IActionResult GetCustomerByID(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            return Ok(customer);
        }
        [HttpGet("get/email/{email}")]
        public IActionResult GetCustomerByEmail(string email)
        {
            var customer = _customerRepository.GetCustomerByEmail(email);
            return Ok(customer);
        }

        [HttpPost("create")]
        public IActionResult AddCustomer(Customer customer)
        {
            var newCustomer = _customerRepository.addCustomer(customer);
            return Ok(newCustomer);
        }
        [HttpPut("update/{username}")]
        public IActionResult UpdateCustomer(string username, Customer customer)
        {
            var updateCustomer = _customerRepository.updateCustomer(username, customer);
            return Ok(updateCustomer);
        }
        [HttpPut("update/{username}/password/{password}")]
        public IActionResult UpdatePassword(string username, string password)
        {
            var updatePassword = _customerRepository.updatePassworkByUser(username, password);
            return Ok(updatePassword);
        }
    }
}
