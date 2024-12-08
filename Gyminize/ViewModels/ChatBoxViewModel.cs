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
    /// <summary>
    /// ViewModel cho hộp thoại chat, quản lý tin nhắn và tương tác với mô hình AI.
    /// </summary>
    public partial class ChatBoxViewModel : ObservableObject
    {
        /// <summary>
        /// Danh sách các tin nhắn trong cuộc trò chuyện.
        /// </summary>
        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get => messages;
            set => SetProperty(ref messages, value);
        }

        private string inputBox;
        /// <summary>
        /// Nội dung của hộp văn bản nhập liệu.
        /// </summary>
        public string InputBox
        {
            get => inputBox;
            set => SetProperty(ref inputBox, value);
        }

        /// <summary>
        /// Lệnh gửi yêu cầu từ người dùng.
        /// </summary>
        public ICommand SendRequest { get; set; }

        /// <summary>
        /// Đối tượng GoogleAI để sử dụng API.
        /// </summary>
        public GoogleAI GoogleAI { get; set; }

        /// <summary>
        /// Mô hình generative AI để tạo nội dung.
        /// </summary>
        public GenerativeModel Model { get; set; }
        /// <summary>
        /// Cấu hình cho việc sinh nội dung.
        /// </summary>
        public GenerationConfig Generateconfig { get; set; }

        /// <summary>
        /// Constructor khởi tạo ChatBoxViewModel với cấu hình ban đầu.
        /// </summary>
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

        /// <summary>
        /// Lệnh gửi tin nhắn từ người dùng và nhận phản hồi từ mô hình AI.
        /// </summary>
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
                        if (response == null || string.IsNullOrEmpty(response.Text))
                        {
                            continue; 
                        }
                        fullResponse += response.Text;
                    }

                    await Task.Delay(20);

                    botMessage.Content = fullResponse;
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

    /// <summary>
    /// Lớp đại diện cho một tin nhắn trong cuộc trò chuyện.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Người gửi tin nhắn (User hoặc GymBo).
        /// </summary>
        public string Sender
        {
            get; set;
        }

        /// <summary>
        /// Nội dung tin nhắn.
        /// </summary>
        public string Content
        {
            get; set;
        }
    }
}
