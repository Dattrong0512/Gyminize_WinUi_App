using Newtonsoft.Json;
using System.Net.Http.Headers;

using System.Text;

namespace Gyminize.Services
{
    /// <summary>
    /// Lớp này chịu trách nhiệm cung cấp dịch vụ hoàn thành cuộc trò chuyện từ Google AI Gemini.
    /// </summary>
    public sealed class GoogleAIGeminiChatCompletionService
    {
        private readonly HttpClient _httpClient; // Đối tượng HttpClient dùng để gửi yêu cầu HTTP.
        private readonly string _apiKey = "AIzaSyCrZDT2AzdOwS8qTnp20T38qDW931uLnqY"; // Khóa API để xác thực với Google AI.

        /// <summary>
        /// Khởi tạo đối tượng GoogleAIGeminiChatCompletionService.
        /// </summary>
        public GoogleAIGeminiChatCompletionService()
        {
            _httpClient = new HttpClient(); // Khởi tạo HttpClient để sử dụng trong các yêu cầu HTTP.
        }

        /// <summary>
        /// Gửi yêu cầu đến API Google AI Gemini để nhận phản hồi từ cuộc trò chuyện dựa trên văn bản đầu vào (prompt).
        /// </summary>
        /// <param name="prompt">Văn bản đầu vào mà người dùng gửi cho AI.</param>
        /// <returns>Trả về nội dung phản hồi từ AI dưới dạng chuỗi JSON.</returns>
        public async Task<string> GetChatCompletionAsync(string prompt)
        {
            try
            {
                var url = "https://gemini.googleapis.com/v1/chat/completions";

                var requestBody = new
                {
                    model = "gemini-1",
                    messages = new[] 
                    {
                        new { role = "user", content = prompt }
                    }
                };

                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                var response = await _httpClient.PostAsync(url, content);

                // Kiểm tra mã trạng thái HTTP trả về
                if (response.IsSuccessStatusCode)
                {
                    // Đọc và trả về nội dung phản hồi nếu thành công
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    // Ném ra ngoại lệ nếu mã trạng thái không thành công
                    throw new Exception("API call failed with status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // Trả về thông báo lỗi nếu có ngoại lệ
                return $"Error: {ex.Message}";
            }
        }
    }
}