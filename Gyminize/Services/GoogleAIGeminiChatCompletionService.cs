using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gyminize.Services
{
    public sealed class GoogleAIGeminiChatCompletionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "AIzaSyCrZDT2AzdOwS8qTnp20T38qDW931uLnqY";

        public GoogleAIGeminiChatCompletionService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetChatCompletionAsync(string prompt)
        {
            try
            {
                // Cấu hình URL và body
                var url = "https://gemini.googleapis.com/v1/chat/completions"; // URL API của Gemini
                var requestBody = new
                {
                    model = "gemini-1",  // Bạn có thể thay đổi model nếu cần
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };

                // Chuyển đổi request body thành JSON
                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Thêm header Authorization
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                // Gửi request và nhận response
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;  // Trả về nội dung phản hồi từ API
                }
                else
                {
                    throw new Exception("API call failed with status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return $"Error: {ex.Message}";
            }
        }
    }
}
