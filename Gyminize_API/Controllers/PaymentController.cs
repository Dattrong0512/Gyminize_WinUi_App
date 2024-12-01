using Gyminize_API.Data.Model;
using Gyminize_API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace Gyminize_API.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentRepository _paymentRepository;
        public PaymentController(PaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        [HttpGet("get/payment/{order_id:int}")]
        public IActionResult GetPaymentByOrderId(int order_id)
        {
            var payment = _paymentRepository.GetPaymentByOrderID(order_id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
        [HttpPost("create/")]
        public IActionResult AddPayment([FromBody] Payment payment)
        {
            if (payment == null)
            {
                return BadRequest("Payment data is null.");
            }

            try
            {
                var createdPayment = _paymentRepository.AddPayment(payment);

                if (createdPayment == null)
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }
                return Ok(createdPayment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}


