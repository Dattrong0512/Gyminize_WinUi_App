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
[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly IVnPayService _vpnPayService;
    private readonly EntityDatabaseContext _context;
    private Payment payment;
    public CartController(IVnPayService vpnPayService, EntityDatabaseContext context)
    {
        _vpnPayService = vpnPayService;
        _context = context;

        payment = new Payment();

    }
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

    [HttpGet("paymentcallback")]
    public async Task<IActionResult> PaymentCallback()
    {
        var response = _vpnPayService.PaymentExcute(Request.Query);

        // Kiểm tra nếu response là null hoặc mã phản hồi không phải là "00" (thất bại)
        if (response == null || response.VnPayResponseCode != "00")
        {
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

        // Chuyển đổi OrderId từ string sang int một cách an toàn
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

        // Tìm đơn hàng trong cơ sở dữ liệu theo orderId
        var order = _context.OrdersEntity
            .Where(x => x.orders_id == orderId)
            .FirstOrDefault();

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
        // Cập nhật trạng thái đơn hàng (thành công)
        order.status = "Completed";
        _context.SaveChanges();
        // Trả về giao diện thành công với mã hóa UTF-8
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
    [HttpGet("check-payment-status/{orderId}")]
    public IActionResult CheckPaymentStatus(int orderId)
    {
        var order = _context.OrdersEntity.FirstOrDefault(x => x.orders_id == orderId);

        if (order.status == "Not Payment")
        {
            return Ok("Đơn hàng chưa thanh toán");
        }
        else if(order.status == "Pending")
        {
            return Ok("Đơn hàng đang thanh toán");
        }
        var payment = _context.PaymentEntity.FirstOrDefault(x => x.orders_id == orderId);
        return Ok(new
        {
            status = payment.payment_status,
            orderId = payment.orders_id,
            paymentAmount = payment.payment_amount
        });
    }

}

