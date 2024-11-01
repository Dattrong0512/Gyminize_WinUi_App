using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
namespace Gyminize_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerhealthController(CustomerHealthRepository repository) : ControllerBase
    {
        private readonly CustomerHealthRepository _repository = repository;
        [HttpGet]
        public IActionResult GetAllCustomerHealth()
        {
            try
            {
                var allCustomerHealth = _repository.GetAllCustomerHealth();
                return Ok(allCustomerHealth);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        [HttpGet("get/{customerId:int}")]
        public IActionResult GetCustomerHealthByCustomerId(int customerId)
        {
            var customerHealth = _repository.GetCustomerHealthByCustomerId(customerId);
            return Ok(customerHealth);
        }

        [HttpPost("create")]
        public IActionResult AddCustomerHealth(Customer_health customerhealth)
        {
            var newCustomerHealth = _repository.AddCustomerHealth(customerhealth);
            return Ok(newCustomerHealth);
        }

    }
}

