using Gyminize_API.Data.Model;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gyminize_API.Controllers;

/// <summary>
/// Controller quản lý các hoạt động liên quan đến thanh toán, bao gồm lấy thông tin thanh toán theo ID đơn hàng và tạo thanh toán mới.
/// </summary>
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentRepository _paymentRepository;

    /// <summary>
    /// Khởi tạo controller với repository để quản lý các hoạt động thanh toán.
    /// </summary>
    /// <param name="paymentRepository">Repository chứa các phương thức thao tác với dữ liệu thanh toán</param>
    public PaymentController(PaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    /// <summary>
    /// Lấy thông tin thanh toán dựa trên ID đơn hàng.
    /// </summary>
    /// <param name="order_id">ID đơn hàng cần lấy thông tin thanh toán</param>
    /// <returns>Trả về thông tin thanh toán nếu tìm thấy, hoặc trả về lỗi 404 nếu không tìm thấy</returns>
    [HttpGet("get/payment/{order_id:int}")]
    public IActionResult GetPaymentByOrderId(int order_id)
    {
        var payment = _paymentRepository.GetPaymentByOrderID(order_id);
        if (payment == null)
        {
            return NotFound(); // Trả về lỗi 404 nếu không tìm thấy thanh toán
        }
        return Ok(payment); // Trả về thông tin thanh toán nếu tìm thấy
    }

    /// <summary>
    /// Tạo một thanh toán mới.
    /// </summary>
    /// <param name="payment">Thông tin thanh toán cần thêm vào hệ thống</param>
    /// <returns>Trả về thông tin thanh toán đã được tạo nếu thành công, hoặc lỗi nếu có vấn đề xảy ra</returns>
    [HttpPost("create/")]
    public IActionResult AddPayment([FromBody] Payment payment)
    {
        if (payment == null)
        {
            return BadRequest("Payment data is null."); // Trả về lỗi 400 nếu dữ liệu thanh toán bị null
        }

        try
        {
            var createdPayment = _paymentRepository.AddPayment(payment);

            if (createdPayment == null)
            {
                return StatusCode(500, "A problem happened while handling your request."); // Trả về lỗi 500 nếu gặp sự cố khi xử lý
            }
            return Ok(createdPayment); // Trả về thông tin thanh toán đã tạo
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}"); // Trả về lỗi 500 nếu có lỗi xảy ra trong quá trình xử lý
        }
    }
}
