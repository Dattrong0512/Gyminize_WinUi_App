using Gyminize_API.Data.Model;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gyminize_API.Controllers
{
    /// \class OrderDetailController
    /// \brief Lớp điều khiển cho các API chi tiết đơn hàng.
    ///
    /// Lớp này cung cấp các API để thêm, cập nhật và xóa chi tiết đơn hàng.
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private readonly OrderDetailRepository _orderDetailRepository;

        /// \brief Khởi tạo một đối tượng OrderDetailController.
        /// \param orderDetailRepository Đối tượng repository để quản lý chi tiết đơn hàng.
        public OrderDetailController(OrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        /// \brief API để thêm hoặc cập nhật chi tiết đơn hàng.
        /// \param orderdetail Đối tượng chi tiết đơn hàng cần thêm hoặc cập nhật.
        /// \return Trả về đối tượng chi tiết đơn hàng đã được thêm hoặc cập nhật.
        [HttpPost("add")]
        public IActionResult UpdateOrAddOrderDetail([FromBody] Orderdetail orderdetail)
        {
            var AddOrderDetail = _orderDetailRepository.AddOrUpdateOrderdetail(orderdetail.orders_id,
                orderdetail.product_amount, orderdetail.Product);
            return Ok(AddOrderDetail);
        }

        /// \brief API để cập nhật số lượng sản phẩm trong chi tiết đơn hàng.
        /// \param orderdetail Đối tượng chi tiết đơn hàng cần cập nhật.
        /// \return Trả về đối tượng chi tiết đơn hàng đã được cập nhật.
        [HttpPut("update/number")]
        public IActionResult UpdateNumberOrderDetail([FromBody] Orderdetail orderdetail)
        {
            var UpdateOrderDetail = _orderDetailRepository.OnlyUpdateOrderDetail(orderdetail.orderdetail_id,
                orderdetail.product_amount);
            return Ok(UpdateOrderDetail);
        }

        /// \brief API để xóa chi tiết đơn hàng.
        /// \param orderdetail Đối tượng chi tiết đơn hàng cần xóa.
        /// \return Trả về true nếu xóa thành công, ngược lại trả về false.
        [HttpDelete("delete")]
        public IActionResult DeleteOrderDetail([FromBody] Orderdetail orderdetail)
        {
            var DeleteOrderDetail = _orderDetailRepository.DeleteFromOrderDetail(orderdetail.orders_id, orderdetail.Product);
            return Ok(DeleteOrderDetail);
        }
    }
    
}
