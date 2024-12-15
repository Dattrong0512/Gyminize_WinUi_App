using Gyminize_API.Data;
using Gyminize_API.Data.Model;

namespace Gyminize_API.Services.PaymentServices;
public interface IVnPayService
{
    string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
    VnPaymentResponseModel PaymentExcute(IQueryCollection collections);
}
