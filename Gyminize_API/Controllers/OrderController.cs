using Gyminize_API.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Gyminize_API.Controllers
{
    /// \class OrderController
    /// \brief Lớp điều khiển cho các API đơn hàng.
    ///
    /// Lớp này cung cấp các API để lấy, thêm, và cập nhật đơn hàng.
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrdersRepository _ordersRepository;

        /// \brief Khởi tạo một đối tượng OrderController.
        /// \param ordersRepository Đối tượng repository để quản lý đơn hàng.
        public OrderController(OrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        /// \brief API để lấy tất cả các đơn hàng của một khách hàng theo ID.
        /// \param customerID ID của khách hàng.
        /// \return Trả về danh sách tất cả các đơn hàng của khách hàng.
        [HttpGet("get/customerId/All/{customerID:int}")]
        public IActionResult GetAllOrdersByCustomerID(int customerID)
        {
            var allOrders = _ordersRepository.GetAllOrderByCustomerId(customerID);
            return Ok(allOrders);
        }

        /// \brief API để lấy đơn hàng của một khách hàng theo ID.
        /// \param customer_id ID của khách hàng.
        /// \return Trả về đơn hàng của khách hàng.
        [HttpGet("get/customer_id/{customer_id}")]
        public IActionResult GetOrdersByCustomerId(int customer_id)
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

        /// \brief API để thêm một đơn hàng mới.
        /// \param order Đối tượng đơn hàng cần thêm.
        /// \return Trả về đối tượng đơn hàng đã được thêm.
        [HttpPost("add")]
        public IActionResult AddOrder([FromBody] Orders order)
        {
            if (order == null)
            {
                return BadRequest("Order data is null.");
            }

            try
            {
                var createdOrder = _ordersRepository.AddOrder(order);
                return Ok(createdOrder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// \brief API để cập nhật trạng thái của một đơn hàng.
        /// \param orders_id ID của đơn hàng.
        /// \param status Trạng thái mới của đơn hàng.
        /// \return Trả về đối tượng đơn hàng đã được cập nhật trạng thái.
        [HttpPost("update/orders_id/{orders_id:int}/status/{status}")]
        public IActionResult UpdateStatusOrder(int orders_id, string status)
        {
            var UpdateStatus = _ordersRepository.UpdateStatusOrder(orders_id, status);
            return Ok(UpdateStatus);
        }
    }
}


