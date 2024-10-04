// Tệp Json.cs chứa các phương thức tiện ích để chuyển đổi giữa chuỗi JSON và đối tượng.
using Newtonsoft.Json;

namespace Gyminize.Core.Helpers;

public static class Json
{
    // Phương thức bất đồng bộ để chuyển đổi chuỗi JSON thành đối tượng kiểu T.
    public static async Task<T> ToObjectAsync<T>(string value)
    {
        return await Task.Run<T>(() =>
        {
            // Sử dụng JsonConvert để giải mã chuỗi JSON thành đối tượng kiểu T.
            return JsonConvert.DeserializeObject<T>(value);
        });
    }

    // Phương thức bất đồng bộ để chuyển đổi đối tượng thành chuỗi JSON.
    public static async Task<string> StringifyAsync(object value)
    {
        return await Task.Run<string>(() =>
        {
            // Sử dụng JsonConvert để mã hóa đối tượng thành chuỗi JSON.
            return JsonConvert.SerializeObject(value);
        });
    }
}