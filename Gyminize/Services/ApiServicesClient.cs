using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gyminize.Contracts.Services;
using Gyminize.Core.Services;
using Gyminize.Services;
public class ApiServicesClient : IApiServicesClient
{
    /// <summary>
    /// Gọi API GET và trả về kết quả dạng T.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu trả về.</typeparam>
    /// <param name="endpoint">Endpoint của API.</param>
    /// <returns>Kết quả từ API.</returns>
    public T Get<T>(string endpoint)
    {
        return ApiServices.Get<T>(endpoint);
    }

    /// <summary>
    /// Gửi dữ liệu dạng POST tới API và trả về kết quả dạng T.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu trả về.</typeparam>
    /// <param name="endpoint">Endpoint của API.</param>
    /// <param name="data">Dữ liệu gửi đi.</param>
    /// <returns>Kết quả từ API.</returns>
    public T Post<T>(string endpoint, object data)
    {
        return ApiServices.Post<T>(endpoint, data);
    }

    /// <summary>
    /// Cập nhật dữ liệu bằng phương thức PUT và trả về kết quả dạng T.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu trả về.</typeparam>
    /// <param name="endpoint">Endpoint của API.</param>
    /// <param name="data">Dữ liệu gửi đi.</param>
    /// <returns>Kết quả từ API.</returns>
    public T Put<T>(string endpoint, object data)
    {
        return ApiServices.Put<T>(endpoint, data);
    }

    /// <summary>
    /// Gửi yêu cầu DELETE tới API và trả về trạng thái thành công hay không.
    /// </summary>
    /// <param name="endpoint">Endpoint của API.</param>
    /// <param name="data">Dữ liệu gửi đi (nếu có).</param>
    /// <returns>True nếu thành công, False nếu thất bại.</returns>
    public bool Delete(string endpoint, object data)
    {
        return ApiServices.Delete(endpoint, data);
    }
}
