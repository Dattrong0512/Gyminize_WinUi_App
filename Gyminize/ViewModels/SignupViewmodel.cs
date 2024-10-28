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
        private string confirmPassword;

        [ObservableProperty]
        private bool isAgree;

        private readonly INavigationService _navigationService;
        public ICommand SignupCommand
        {
            get;
        }

        public SignupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
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
        private void OnSignUp()
        {
            try
            {
                if (!string.IsNullOrEmpty(Username) && Password == ConfirmPassword )
                {
                    if(checkExistCustomer(Username))
                    {
                        SignupStatus = "Lỗi đăng ký: Tài khoản đã tồn tại!";
                    }
                    else
                    {
                        if (IsAgree == true)
                        {
                            PostCustomer(Username, Password);
                            var pageKey = typeof(Guide1ViewModel).FullName;
                            if (pageKey != null)
                            {
                                _navigationService.NavigateTo(pageKey, Username);
                            }
                            SignupStatus = $"Đăng ký thành công cho {Username}!";
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
            Customer customer = new Customer(username,1,username,password);
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7141/");
            var json = System.Text.Json.JsonSerializer.Serialize(customer);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = client.PostAsync("api/Customer/create", content).Result;
        }
        private void output(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
