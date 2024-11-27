using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Mscc.GenerativeAI;

namespace Gyminize.ViewModels
{
    public partial class ChatBoxViewModel : ObservableObject
    {
        // Thuộc tính để binding trong giao diện
        [ObservableProperty]
        private string responseText;

        // Thuộc tính cho InputBox (để binding vào TextBox)
        [ObservableProperty]
        private string inputBox;

        // Command để gửi yêu cầu từ người dùng
        public ICommand SendRequest
        {
            get; set;
        }

        public GoogleAI GoogleAI
        {
            get; set;
        }
        public GenerativeModel Model
        {
            get; set;
        }

        // Constructor
        public ChatBoxViewModel()
        {
            // Khởi tạo khi ViewModel được tạo ra
            GoogleAI = new GoogleAI(apiKey: "AIzaSyCrZDT2AzdOwS8qTnp20T38qDW931uLnqY"); // Thử với API key trực tiếp
            Model = GoogleAI.GenerativeModel(model: "gemini-1.5-flash-8b");
            SendRequest = new RelayCommand(SendMessage);
        }

        // Command thực hiện gửi yêu cầu từ người dùng
        private async void SendMessage()
        {
            try
            {
                ResponseText = string.Empty; // Đặt lại ResponseText để xóa câu trả lời trước đó
                // Tạo cấu hình cho việc sinh nội dung (có thể thêm tham số mới để làm yêu cầu duy nhất)
                var generationConfig = new GenerationConfig
                {
                    Temperature = 0.7f,
                    TopP = 0.9f,
                };
                var responseStream = Model.GenerateContentStream(InputBox, generationConfig);
             

                // Kiểm tra nếu responseStream không phải null
                if (responseStream == null)
                {
                    ResponseText = "Error: Response stream is null.";
                    return;
                }

                // Kiểm tra kiểu trả về từ API (IAsyncEnumerable<GenerateContentResponse>)
                if (responseStream is IAsyncEnumerable<GenerateContentResponse>)
                {
                    // Lặp qua dữ liệu trả về stream
                    await foreach (var response in responseStream)
                    {
                        // Kiểm tra nếu response là null hoặc không có text
                        if (response == null || string.IsNullOrEmpty(response.Text))
                        {
                            continue; // Bỏ qua nếu không có dữ liệu hợp lệ
                        }

                        // Tách văn bản thành các từ và gửi từng từ một
                        var words = response.Text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var word in words)
                        {
                            // Cập nhật responseText mỗi khi nhận được từ
                            ResponseText = (ResponseText ?? "") + word + " ";  // Kiểm tra null và thêm từ vào

                            // Optional: Bạn có thể thêm một delay nếu muốn để làm cho từng từ xuất hiện từ từ
                            await Task.Delay(20);  // Delay 100ms (tùy chỉnh để tạo hiệu ứng xuất hiện từ từ)
                        }
                    }
                }
                else
                {
                    // Nếu không phải stream, log lỗi hoặc thông báo về dữ liệu trả về
                    ResponseText = "Error: Response is not a stream.";
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ResponseText = "Error in SendMessage: " + ex.Message;
            }
        }
    }
}
