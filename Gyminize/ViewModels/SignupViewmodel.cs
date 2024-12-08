using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Models;
using Windows.Gaming.Input.ForceFeedback;
using System.Net.Http;
using System.Net;
using Gyminize.ViewModels;
using Gyminize.Contracts.Services;
using Gyminize.Services;
using Gyminize.Helpers;
namespace Gyminize.ViewModels
{

    /// \brief ViewModel xử lý các hoạt động đăng ký người dùng.
    public partial class SignupViewModel : ObservableObject
    {
        [ObservableProperty]
        private string signupStatus;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private bool isAgree;

        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IApiServicesClient _apiService;

         public ICommand SignupCommand { get; }
        public ICommand GoBackCommand { get; }


        /// \brief Hàm khởi tạo cho SignupViewModel.
        /// \param navigationService Dịch vụ điều hướng.
        /// \param dialogService Dịch vụ hiển thị hộp thoại.
        /// \param apiServicesClient Dịch vụ kết nối API.
        public SignupViewModel(INavigationService navigationService, IDialogService dialogService, IApiServicesClient apiServicesClient)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiServicesClient;
            SignupCommand = new RelayCommand(OnSignUp);
            GoBackCommand = new RelayCommand(OnGoBack);
        }

        /// \brief Kiểm tra sự tồn tại của tài khoản người dùng.
        /// \param username Tên tài khoản cần kiểm tra.
        /// \return `true` nếu tài khoản tồn tại, ngược lại `false`.
        private bool checkExistCustomer(string username)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7141/");
            var response = client.GetAsync("api/Customer/get/username/" + username).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            return false;
        }

        /// \brief Xử lý sự kiện khi nhấn nút Đăng ký.
        private async void OnSignUp()
        {
            try
            {
                var result = _apiService.Get<Customer>("api/Customer/get/email/" + email);
                if (!string.IsNullOrEmpty(Username) && Password == ConfirmPassword)
                {
                    if (checkExistCustomer(Username))
                    {
                        SignupStatus = "Lỗi đăng ký: Tài khoản đã tồn tại!";
                    }
                    else if (Username.Length < 6 || Password.Length < 6)
                    {
                        SignupStatus = "Lỗi đăng ký: Tài khoản và mật khẩu phải có ít nhất 6 ký tự!";
                    }
                    else if (!ValidateHelper.IsValidEmail(email))
                    {
                        SignupStatus = "Lỗi đăng ký: Email không hợp lệ";
                    }
                    else if (result != null) // Check email da ton tai o day
                    {
                        SignupStatus = "Lỗi đăng ký: Email đã tồn tại";
                    }
                    else
                    {
                        if (IsAgree == true)
                        {   
                            string recipientEmail = email;
                            Random random = new Random();
                            string verificationCode = random.Next(0, 10000).ToString("D4");//Tạo mã xác thực random
                            sendVerificationCode(recipientEmail, verificationCode);
                            if (await _dialogService.ShowVerificationDialogAsync(recipientEmail, verificationCode) == true)
                            {
                                PostCustomer(Username, Password, recipientEmail);
                                var pageKey = typeof(Guide1ViewModel).FullName;
                                if (pageKey != null)
                                {
                                    _navigationService.NavigateTo(pageKey, Username);
                                }
                                SignupStatus = $"Đăng ký thành công cho {Username}!";
                            }
                            else
                            {
                                // do nothing
                            }
                        }
                        else
                        {
                            SignupStatus = "Lỗi đăng ký: Bạn chưa đồng ý với điều khoản!";
                        }
                    }
                }
                else
                {

                    SignupStatus = "Lỗi đăng ký: Mật khẩu và xác minh mật khẩu không khớp hoặc thông tin trống!";
                }

                output(SignupStatus);
            }
            catch (Exception ex)
            {
                SignupStatus = "Đăng ký thất bại: " + ex.Message;
                output(SignupStatus);
            }
        }

        /// \brief Điều hướng quay lại trang đăng nhập.
        private void OnGoBack()
        {
            var pageKey = typeof(SigninViewmodel).FullName;
            if (pageKey != null)
            {
                _navigationService.NavigateTo(pageKey);
            }
        }

        /// \brief Thêm thông tin người dùng mới vào hệ thống.
        /// \param username Tên tài khoản.
        /// \param password Mật khẩu.
        /// \param email Email đăng ký.
        private void PostCustomer(string username, string password, string email)
        {
            Customer customer = new Customer(username, 1, username, password, 1, email);
            Console.WriteLine(customer);

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7141/");

            try
            {
                // Serialize customer object to JSON
                var json = System.Text.Json.JsonSerializer.Serialize(customer);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send the POST request
                var response = client.PostAsync("api/Customer/create", content).Result;

                // Check if the response indicates success
                if (response.IsSuccessStatusCode)
                {
                    output("Customer created successfully.");
                }
                else
                {
                    // Read the error content if the request failed
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    output($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    output($"Error Details: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                output($"An error occurred: {ex.Message}");
            }
        }

        /// \brief Ghi log hoặc hiển thị thông báo.
        /// \param message Nội dung thông báo.
        private void output(string message)
        {
            Debug.WriteLine(message);
        }

        /// \brief Gửi mã xác thực qua email.
        /// \param email Email người nhận.
        /// \param code Mã xác thực
        public async void sendVerificationCode(string email, string code)
        {
            IEmailSender emailSender = new EmailSender();
            string subject = "Mã xác thực cho Gyminize App";
            // Nội dung email
            string body = $"<h1>Mã xác thực của bạn là: {code}</h1>" +
            $"<h1>Vui lòng không chia sẻ cho bất kì ai khác</h1>"; // HTML

            // Gửi email
            try
            {
                await emailSender.SendEmailAsync(email, subject, body);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
