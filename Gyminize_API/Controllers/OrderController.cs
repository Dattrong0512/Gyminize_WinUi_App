using Gyminize_API.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Gyminize_API.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrdersRepository _ordersRepository;
        public OrderController(OrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        [HttpGet("get/customerId/All/{customerID:int}")]
        public IActionResult GetAllOrdersByCustomerID(int customerID)
        {
            var allOrders = _ordersRepository.GetAllOrderByCustomerId(customerID);
            return Ok(allOrders); 
        }
        [HttpGet("get/customer_id/{customer_id}")]
        public IActionResult GetAllOrdersByCustomerId(int customer_id)
        {
            try
            {
                var Orders = _ordersRepository.GetOrderByCustomerId(customer_id);
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}


