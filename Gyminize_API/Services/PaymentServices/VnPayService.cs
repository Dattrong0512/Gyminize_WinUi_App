using System.Diagnostics;
using Gyminize_API.Data;
using Gyminize_API.Data.Model;
using Gyminize_API.ViewModel;

namespace Gyminize_API.Services.PaymentServices;

public class VnPayService : IVnPayService
{
    private readonly IConfiguration _config;
    public VnPayService(IConfiguration config)
    {
        _config = config;
    }
    public long tick;
    string IVnPayService.CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
    {
        tick = DateTime.Now.Ticks;
        var vnpay = new VnPayLibrary();
        vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
        vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
        vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
        vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không 
        //mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND
        //(một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần(khử phần thập phân), sau đó gửi sang VNPAY
        //là: 10000000
        vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
        vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
        vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
        vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);

        vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + model.OrderID.ToString());
        vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
        vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
        vnpay.AddRequestData("vnp_TxnRef", model.OrderID.ToString() + tick.ToString()); // Mã tham chiếu của giao dịch tại hệ 
                                                                                        //thống của merchant.Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY.Không được
                                                                                        //        trùng lặp trong ngày
        var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);
        Debug.WriteLine(paymentUrl);
        return paymentUrl;

    }
    VnPaymentResponseModel IVnPayService.PaymentExcute(IQueryCollection collections)
    {
        var vnpay = new VnPayLibrary();
        foreach (var (key, value) in collections)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
            {
                vnpay.AddResponseData(key, value.ToString());
            }
        }
        var orderIdString = vnpay.GetResponseData("vnp_TxnRef").Substring(0, vnpay.GetResponseData("vnp_TxnRef").Length - tick.ToString().Length);
        var vnp_OrderId = Convert.ToInt64(orderIdString);
        var vnp_TransactionID = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
        var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
        var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
        var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
        var checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
        if (!checkSignature)
        {
            return new VnPaymentResponseModel { Success = false };
        }
        return new VnPaymentResponseModel
        {
            Success = true,
            PaymentMethod = "VnPay",
            OrderDescription = vnp_OrderInfo,
            OrderId = vnp_OrderId.ToString(),
            TransactionId = vnp_TransactionID.ToString(),
            Token = vnp_SecureHash,
            VnPayResponseCode = vnp_ResponseCode

        };
    }
}
