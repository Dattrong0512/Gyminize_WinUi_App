using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;  // Sửa lại 'Model' thành 'Models' để nhất quán
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gyminize_API.Controllers
{
    /// <summary>
    /// Controller để quản lý dữ liệu sức khỏe của khách hàng.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerhealthController : ControllerBase  // Sửa constructor cho đúng cú pháp
    {
        private readonly CustomerHealthRepository _repository;

        // Sửa cú pháp constructor
        public CustomerhealthController(CustomerHealthRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Lấy dữ liệu sức khỏe của khách hàng theo ID.
        /// </summary>
        /// <param name="customerId">ID của khách hàng.</param>
        /// <returns>Dữ liệu sức khỏe của khách hàng.</returns>
        [HttpGet("get/{customerId:int}")]
        public IActionResult GetCustomerHealthByCustomerId(int customerId)
        {
            var customerHealth = _repository.GetCustomerHealthByCustomerId(customerId);
            return Ok(customerHealth);
        }

        /// <summary>
        /// Thêm dữ liệu sức khỏe mới cho khách hàng.
        /// </summary>
        /// <param name="customerhealth">Dữ liệu sức khỏe của khách hàng.</param>
        /// <returns>Dữ liệu sức khỏe mới được thêm.</returns>
        [HttpPost("create")]
        public IActionResult AddCustomerHealth(Customer_health customerhealth)
        {
            var newCustomerHealth = _repository.AddCustomerHealth(customerhealth);
            return Ok(newCustomerHealth);
        }

        /// <summary>
        /// Cập nhật cân nặng của khách hàng.
        /// </summary>
        /// <param name="weight">Cân nặng mới của khách hàng.</param>
        /// <param name="customerID">ID của khách hàng.</param>
        /// <returns>Dữ liệu sức khỏe đã được cập nhật của khách hàng.</returns>
        [HttpPut("update/{customerID:int}/weight/{weight:int}")]
        public IActionResult UpdateWeightCustomer(int weight, int customerID)
        {
            var updateWeightCustomer = _repository.UpdateWeightCustomer(weight, customerID);
            return Ok(updateWeightCustomer);
        }
    }
}