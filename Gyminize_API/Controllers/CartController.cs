using System.Diagnostics;
using System.Net;
using Gyminize_API.Data;
using Gyminize_API.Data.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using Gyminize_API.Services.PaymentServices;
using Microsoft.AspNetCore.SignalR;

namespace Gyminize_API.Controllers;
/// <summary>
/// Controller xử lý các yêu cầu liên quan đến giỏ hàng và thanh toán.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly IVnPayService _vpnPayService;
    private readonly EntityDatabaseContext _context;
    private Payment payment;

    /// <summary>
    /// Khởi tạo đối tượng CartController.
    /// </summary>
    /// <param name="vpnPayService">Dịch vụ thanh toán VnPay.</param>
    /// <param name="context">Ngữ cảnh cơ sở dữ liệu.</param>
    public CartController(IVnPayService vpnPayService, EntityDatabaseContext context)
    {
        _vpnPayService = vpnPayService;
        _context = context;
        payment = new Payment();
    }

    /// <summary>
    /// Tạo và xử lý URL thanh toán VnPay cho đơn hàng.
    /// </summary>
    /// <param name="orderId">ID của đơn hàng.</param>
    /// <returns>Trả về đối tượng Payment với trạng thái "Pending".</returns>
    [HttpPost("createPaymentVnpay/orderId/{orderId:int}")]
    public IActionResult CreateAndHandlePaymentUrl(int orderId)
    {
        Process myprocess = new Process();
        myprocess.StartInfo.UseShellExecute = true;
        var orders = _context.OrdersEntity
            .Where(x => x.orders_id == orderId)
            .FirstOrDefault();

        var VnPayModel = new VnPaymentRequestModel
        {
            Amount = ((double)orders.total_price),
            CreatedDate = DateTime.Now,
            Description = "Thanh toán đơn hàng",
            FullName = "Nguyễn Văn A",
            OrderID = orders.orders_id,
        };
        string paymentRequest = _vpnPayService.CreatePaymentUrl(HttpContext, VnPayModel);
        myprocess.StartInfo.FileName = paymentRequest;
        orders.status = "Pending";
        _context.SaveChanges();
        payment.payment_status = "Pending";
        myprocess.Start();

        return Ok(payment);
    }

    /// <summary>
    /// Xử lý callback sau khi thanh toán từ VnPay.
    /// </summary>
    /// <returns>Trả về giao diện HTML thông báo kết quả thanh toán.</returns>
    [HttpGet("paymentcallback")]
    public async Task<IActionResult> PaymentCallback()
    {
        var response = _vpnPayService.PaymentExcute(Request.Query);
        if (!int.TryParse(response.OrderId, out int orderId))
        {
            return Content(@"
            <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>OrderId không hợp lệ</title>
                </head>
                <body>
                    <h2 style='color:red;'>OrderId không hợp lệ!</h2>
                </body>
            </html>", "text/html; charset=utf-8");
        }

        var order = _context.OrdersEntity
            .Where(x => x.orders_id == orderId)
            .FirstOrDefault();

        if (response == null || response.VnPayResponseCode != "00")
        {
            order.status = "Not Payment";
            _context.SaveChanges();
            return Content(@"
            <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Thanh toán thất bại</title>
                </head>
                <body>
                    <h2 style='color:red;'>Thanh toán thất bại!</h2>
                </body>
            </html>", "text/html; charset=utf-8");
        }

        if (order == null)
        {
            return Content(@"
            <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Không tìm thấy đơn hàng</title>
                </head>
                <body>
                    <h2 style='color:red;'>Không tìm thấy đơn hàng!</h2>
                </body>
            </html>", "text/html; charset=utf-8");
        }

        payment.orders_id = orderId;
        payment.payment_date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        payment.payment_method = "VnPay";
        payment.payment_status = "Completed";
        payment.payment_amount = order.total_price;
        payment.transaction_id = response.TransactionId;

        _context.PaymentEntity.Add(payment);
        _context.SaveChanges();
        order.status = "Completed";
        _context.SaveChanges();
        return Content($@"
        <html>
            <head>
                <meta charset='UTF-8'>
                <title>Thanh toán thành công</title>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    h2 {{ color: green; }}
                    p {{ font-size: 16px; }}
                </style>
            </head>
            <body>
                <h2>Thanh toán thành công!</h2>
                <p><strong>Mã thanh toán:</strong> {response.TransactionId}</p>
                <p><strong>Mã đơn hàng:</strong> {orderId}</p>
                <p><strong>Số tiền:</strong> {payment.payment_amount.ToString("N2")} VND</p>
            </body>
        </html>", "text/html; charset=utf-8");
    }

    /// <summary>
    /// Kiểm tra trạng thái thanh toán của đơn hàng.
    /// </summary>
    /// <param name="orderId">ID của đơn hàng.</param>
    /// <returns>Trả về trạng thái thanh toán của đơn hàng.</returns>
    [HttpGet("check-payment-status/{orderId}")]
    public IActionResult CheckPaymentStatus(int orderId)
    {
        var order = _context.OrdersEntity.FirstOrDefault(x => x.orders_id == orderId);

        if (order == null)
        {
            return NotFound(new { message = "Order not found" });
        }

        if (order.status == "Not Payment")
        {
            return Ok(new { message = "Đơn hàng thanh toán thất bại" });
        }
        else if (order.status == "Pending")
        {
            return Ok(new { message = "Đơn hàng đang thanh toán" });
        }
        else if (order.status == "Completed")
        {
            return Ok(new { message = "Đơn hàng thanh toán thành công" });
        }

        var payment = _context.PaymentEntity.FirstOrDefault(x => x.orders_id == orderId);
        if (payment == null)
        {
            return NotFound(new { message = "Payment not found" });
        }

        return Ok(new
        {
            message = "Thông tin thanh toán",
            status = payment.payment_status,
            orderId = payment.orders_id,
            paymentAmount = payment.payment_amount
        });
    }
}

