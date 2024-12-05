using System;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get => messages;
            set => SetProperty(ref messages, value);
        }

        private string inputBox;
        public string InputBox
        {
            get => inputBox;
            set => SetProperty(ref inputBox, value);
        }

        public ICommand SendRequest { get; set; }

        public GoogleAI GoogleAI { get; set; }
        public GenerativeModel Model { get; set; }
        public GenerationConfig Generateconfig { get; set; }

        // Constructor
        public ChatBoxViewModel()
        {
            Messages = new ObservableCollection<Message>();
            GoogleAI = new GoogleAI(apiKey: "AIzaSyCrZDT2AzdOwS8qTnp20T38qDW931uLnqY"); // Thử với API key trực tiếp
            Generateconfig = new GenerationConfig
            {
                Temperature = 0.9f,
                TopP = 1f,
                TopK = 1,
                MaxOutputTokens = 10000,
            };
            Model = GoogleAI.GenerativeModel(model: "gemini-pro");
            SendRequest = new RelayCommand(SendMessage);
        }

        // Command thực hiện gửi yêu cầu từ người dùng
        private async void SendMessage()
        {
            try
            {
                // Thêm tin nhắn của người dùng vào cuộc trò chuyện
                Messages.Add(new Message { Sender = "User", Content = inputBox });
                var responseStream = Model.GenerateContentStream(inputBox, Generateconfig);

                // Kiểm tra nếu responseStream không phải null
                if (responseStream == null)
                {
                    Messages.Add(new Message { Sender = "GymBo", Content = "Error: Response stream is null." });
                    return;
                }

                // Kiểm tra kiểu trả về từ API (IAsyncEnumerable<GenerateContentResponse>)
                if (responseStream is IAsyncEnumerable<GenerateContentResponse> responseStreamEnum)
                {
                    string fullResponse = string.Empty;
                    var botMessage = new Message { Sender = "GymBo", Content = string.Empty };
                    Messages.Add(botMessage);

                    // Lặp qua dữ liệu trả về stream
                    await foreach (var response in responseStreamEnum)
                    {
                        // Kiểm tra nếu response là null hoặc không có text
                        if (response == null || string.IsNullOrEmpty(response.Text))
                        {
                            continue; // Bỏ qua nếu không có dữ liệu hợp lệ
                        }

                        // Cập nhật fullResponse với toàn bộ văn bản nhận được
                        fullResponse += response.Text;
                    }

                    // Cập nhật nội dung của botMessage với toàn bộ văn bản nhận được
                    botMessage.Content = fullResponse;
                    // Notify the UI about the change
                    Messages[Messages.IndexOf(botMessage)] = botMessage;
                }
                else
                {
                    // Nếu không phải stream, log lỗi hoặc thông báo về dữ liệu trả về
                    Messages.Add(new Message { Sender = "GymBo", Content = "Error: Response is not a stream." });
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Messages.Add(new Message { Sender = "GymBo", Content = "Error in SendMessage: " + ex.Message });
            }
        }

    }

    public class Message
    {
        public string Sender { get; set; }
        public string Content { get; set; }
    }
}
