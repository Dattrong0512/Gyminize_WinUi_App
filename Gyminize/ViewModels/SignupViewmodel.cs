using System;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Models;

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

        public ICommand SignupCommand
        {
            get;
        }

        public SignupViewModel()
        {
            SignupCommand = new RelayCommand(OnSignUp);
        }

        private void OnSignUp()
        {
            try
            {
                if (!string.IsNullOrEmpty(Username) && Password == ConfirmPassword)
                {
                    SignupStatus = $"Đăng ký thành công cho {Username}!";
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

        private void output(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
