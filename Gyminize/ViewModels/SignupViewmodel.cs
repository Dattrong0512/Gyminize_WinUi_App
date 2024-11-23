﻿using System;
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

namespace Gyminize.ViewModels
{
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
        public ICommand SignupCommand
        {
            get;
        }

        public SignupViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            SignupCommand = new RelayCommand(OnSignUp);
        }
        private bool checkExistCustomer(string username)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7141/");
            var response = client.GetAsync("api/Customer/get/username/" + username).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else if(response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            return false;   
        }
        private async void OnSignUp()
        {
            try
            {
                if (!string.IsNullOrEmpty(Username) && Password == ConfirmPassword )
                {
                    if(checkExistCustomer(Username))
                    {
                        SignupStatus = "Lỗi đăng ký: Tài khoản đã tồn tại!";
                    } else if( 1 == 2) // Check email da ton tai o day
                    {
                        SignupStatus = "Lỗi đăng ký: Email đã tồn tại";
                    }
                    else
                    {
                        if (IsAgree == true)
                        {
                            string recipientEmail = "huaminhquan111@gmail.com";      
                            Random random = new Random();
                            string verificationCode = random.Next(0, 10000).ToString("D4");//Tạo mã xác thực random
                            sendVerificationCode(recipientEmail, verificationCode);
                            if (await _dialogService.ShowVerificationDialogAsync(recipientEmail,verificationCode) ==  true)
                            {
                                PostCustomer(Username, Password);
                                var pageKey = typeof(Guide1ViewModel).FullName;
                                if (pageKey != null)
                                {
                                    _navigationService.NavigateTo(pageKey, Username);
                                }
                                SignupStatus = $"Đăng ký thành công cho {Username}!";
                            } else
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
        private void PostCustomer(string username, string password)
        {
            Customer customer = new Customer(username, 1, username, password, 1,"trongleviet@gmail.com");
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

        private void output(string message)
        {
            Debug.WriteLine(message);
        }

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
