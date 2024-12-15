using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Text.Json;
using Newtonsoft.Json;
using Gyminize.Models;
using Gyminize.Contracts.Services;
using Gyminize.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.Background;
using Gyminize.Services;
using Windows.Web.AtomPub;
using Gyminize.Core.Services;

using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Net.Mail;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using System.Drawing;
namespace Gyminize.ViewModels;

/// \class SigninViewmodel
/// \brief ViewModel để xử lý các hoạt động đăng nhập.
///
/// ViewModel này quản lý quá trình đăng nhập, bao gồm xác thực người dùng, xử lý OAuth và điều hướng.
public partial class SigninViewmodel : ObservableObject
{

    [ObservableProperty]
    private string loginStatus;
    [ObservableProperty]
    private string username;
    [ObservableProperty]
    private string password;

    // OAuth 2.0 client configuration
    const string clientID = "25264695175-026qgqsmvslrgn5gm2vj9gseu1ugf4ro.apps.googleusercontent.com";
    const string clientSecret = "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"; // Thêm client_secret
    const string redirectURI = "http://localhost:8080";  // Sử dụng URI localhost để nhận phản hồi
    const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
    const string tokenEndpoint = "https://oauth2.googleapis.com/token";
    const string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";

    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IWindowService _windowService;
    private Customer customer;
    private UIElement? _shell = null;
    public ILocalSettingsService _localSettingsService;
    private SolidColorBrush _statusColor;
    private bool rememberMe;

    /// \brief Lấy hoặc đặt màu trạng thái.
    public SolidColorBrush StatusColor
    {
        get => _statusColor;
        set
        {
            if (_statusColor != value)
            {
                _statusColor = value;
                OnPropertyChanged(nameof(StatusColor));
            }
        }
    }

    /// \brief Lấy hoặc đặt tùy chọn ghi nhớ đăng nhập.
    public bool RememberMe
    {
        get => rememberMe;
        set => SetProperty(ref rememberMe, value);
    }

    /// \brief Lệnh đăng nhập bằng Google.
    public ICommand LoginCommandByGoogle { get; }
    
    /// \brief Lệnh đăng nhập bằng thông tin người dùng.
    public ICommand LoginCommandByUser { get; }
    
    /// \brief Lệnh điều hướng đến trang đăng ký.
    public ICommand SignUpNavigateCommand { get; }

    /// \brief Lệnh xử lý quên mật khẩu.
    public ICommand ForgotPasswordProcessingCommand { get; }


    public SigninViewmodel(INavigationService navigationService, ILocalSettingsService localSettings, IDialogService dialogService, IWindowService windowService)
    {
        _windowService = windowService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _windowService.SetIsResizable(false);
        _windowService.SetIsMaximizable(false);
        _windowService.SetTitle("");
        LoginCommandByGoogle = new RelayCommand(OnLoginByGoogle);
        LoginCommandByUser = new RelayCommand(OnLoginByUser);
        SignUpNavigateCommand = new RelayCommand(OnSignUpByUserNavigate);
        ForgotPasswordProcessingCommand = new RelayCommand(OnForgotPasswordProcessing);
        var customer = new Customer();
        _localSettingsService = localSettings;

        var settings = ApplicationData.Current.LocalSettings.Values;

        // Kiểm tra nếu giá trị tồn tại trong LocalSettings
        if (settings.ContainsKey("RememberMe"))
        {
            RememberMe = (bool)settings["RememberMe"];
            Username = (string)settings["Username"];
            Password = (string)settings["Password"];
        }
    }

    /// \brief Lệnh điều hướng đến trang đăng ký.
    private void OnSignUpByUserNavigate()
    {
        var pageKey = typeof(SignupViewModel).FullName;
        if (pageKey != null)
        {
            _navigationService.NavigateTo(pageKey);
        }
    }

    /// \brief Đăng nhập người dùng bằng thông tin đăng nhập.
    private async void OnLoginByUser()
    {

        if (CheckCustomerByGet(Username, Password))
        {
            if (Password != customer.customer_password)
            {
                output("Mật khẩu không đúng!");
            }
            else
            {
                if (CheckCustomerHealthByUsername(Username))
                {
                    if (RememberMe)
                    {
                        var settings = ApplicationData.Current.LocalSettings.Values;

                        // Lưu các giá trị vào LocalSettings
                        settings["RememberMe"] = rememberMe;
                        settings["Username"] = username;
                        settings["Password"] = password;
                    }
                    else
                    {
                        var settings = ApplicationData.Current.LocalSettings.Values;
                        settings["RememberMe"] = rememberMe;
                        settings["Username"] = string.Empty;
                        settings["Password"] = string.Empty;
                    }
                    if (App.MainWindow.Content != null)
                    {
                        var frame = new Frame();
                        _shell = App.GetService<ShellPage>();
                        frame.Content = _shell;
                        App.MainWindow.Content = frame;
                        await _localSettingsService.SaveSettingAsync("customer_id", customer.customer_id);
                        _navigationService.NavigateTo(typeof(HomeViewModel).FullName!);
                    }
                }
                else
                {
                    if (RememberMe)
                    {
                        var settings = ApplicationData.Current.LocalSettings.Values;

                        // Lưu các giá trị vào LocalSettings
                        settings["RememberMe"] = rememberMe;
                        settings["Username"] = username;
                        settings["Password"] = password;
                    }
                    else
                    {
                        var settings = ApplicationData.Current.LocalSettings.Values;
                        settings["RememberMe"] = rememberMe;
                        settings["Username"] = string.Empty;
                        settings["Password"] = string.Empty;
                    }
                    var pageKey = typeof(Guide1ViewModel).FullName;
                    if (pageKey != null)
                    {
                        _navigationService.NavigateTo(pageKey, Username);
                    }
                }
                output("Đăng nhập thành công!");
            }
        }
        else
        {
            output("Tài khoản chưa tồn tại, vui lòng đăng kí");
        }
    }

    /// \brief Đăng nhập người dùng bằng Google OAuth.
    private async void OnLoginByGoogle()
    {
        // Generate state and PKCE values
        string state = randomDataBase64url(32);
        string codeVerifier = randomDataBase64url(32);
        string codeChallenge = base64urlencodeNoPadding(sha256(codeVerifier));
        const string codeChallengeMethod = "S256";

        // Store the state and code_verifier values into local settings
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values["state"] = state;
        localSettings.Values["code_verifier"] = codeVerifier;

        // Create the OAuth 2.0 authorization request URL
        string authorizationRequest = string.Format(
            "{0}?response_type=code&scope=openid%20profile%20email&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}",
            authorizationEndpoint,
            Uri.EscapeDataString(redirectURI),
            clientID,
            state,
            codeChallenge,
            codeChallengeMethod
        );
        // Open the authorization request URL in the browser
        var success = await Launcher.LaunchUriAsync(new Uri(authorizationRequest));
        if (!success)
        {
            await new MessageDialog("Failed to launch OAuth request.").ShowAsync();
        }

        // Start the HTTP listener to receive the callback
        await ListenForOAuthCallback();
    }

    /// \brief Lắng nghe callback OAuth.
    private async Task ListenForOAuthCallback()
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/"); // Listening on port 8080
        listener.Start();


        HttpListenerContext context = await listener.GetContextAsync(); // Wait for the request
        HttpListenerRequest request = context.Request;
        string queryString = request.Url.Query;

        Dictionary<string, string> queryStringParams = queryString.Substring(1).Split('&')
            .ToDictionary(c => c.Split('=')[0], c => Uri.UnescapeDataString(c.Split('=')[1]));

        if (queryStringParams.ContainsKey("error"))
        {
            output($"OAuth authorization error: {queryStringParams["error"]}");
            return;
        }

        if (!queryStringParams.ContainsKey("code") || !queryStringParams.ContainsKey("state"))
        {
            output("Malformed authorization response.");
            return;
        }

        // Gets the Authorization code & state
        string code = queryStringParams["code"];
        string incomingState = queryStringParams["state"];

        // Retrieve the expected state value from local settings (saved when the request was made)
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        string expectedState = (string)localSettings.Values["state"];

        if (incomingState != expectedState)
        {
            output($"Invalid state ({incomingState}) received.");
            return;
        }

        // Reset the state value to avoid replay attacks
        localSettings.Values["state"] = null;

        // Authorization Code is now ready to use!
        //output($"Authorization code: {code}");

        string codeVerifier = (string)localSettings.Values["code_verifier"];
        await ExchangeCodeForTokensAsync(code, codeVerifier);  // Sử dụng mã ủy quyền để lấy token

        // Send response to browser
        HttpListenerResponse response = context.Response;
        string responseString = "<html><body>Authentication complete. You can close this window.</body></html>";
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();

        listener.Stop();
    }


    /// \brief Trao đổi mã ủy quyền lấy token.
    /// \param code Mã ủy quyền.
    /// \param codeVerifier Mã xác minh.
    /// \return Một task đại diện cho hoạt động bất đồng bộ.
    public async Task ExchangeCodeForTokensAsync(string code, string codeVerifier)
    {
        string tokenRequestBody = string.Format(
            "code={0}&redirect_uri={1}&client_id={2}&client_secret={3}&code_verifier={4}&grant_type=authorization_code",
            code,
            Uri.EscapeDataString(redirectURI),
            clientID,
            clientSecret,  // Thêm client_secret vào đây
            codeVerifier
        );

        StringContent content = new StringContent(tokenRequestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

        // Perform the authorization code exchange
        HttpClientHandler handler = new HttpClientHandler { AllowAutoRedirect = true };
        HttpClient client = new HttpClient(handler);

        Debug.WriteLine("Exchanging code for tokens...");
        HttpResponseMessage response = await client.PostAsync(tokenEndpoint, content);
        string responseString = await response.Content.ReadAsStringAsync();
        Debug.WriteLine(responseString);

        if (!response.IsSuccessStatusCode)
        {
            Debug.WriteLine("Authorization code exchange failed.");
            output("Authorization code exchange failed.");
            return;
        }

        // Parse tokens using System.Text.Json
        using (JsonDocument jsonDocument = JsonDocument.Parse(responseString))
        {
            JsonElement root = jsonDocument.RootElement;
            string accessToken = root.GetProperty("access_token").GetString();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            //// Make a call to the Userinfo endpoint
            //output("Making API Call to Userinfo...");
            HttpResponseMessage userinfoResponse = await client.GetAsync(userInfoEndpoint);
            string userinfoResponseContent = await userinfoResponse.Content.ReadAsStringAsync();

            // Output the Userinfo API response
            //output(userinfoResponseContent);
            ProcessingDataLogin(userinfoResponseContent);

        }
    }

    /// \brief Xử lý dữ liệu đăng nhập.
    /// \param DataLogin Dữ liệu đăng nhập.
    private async void ProcessingDataLogin(string DataLogin)
    {
        // Parse JSON response from Google user info
        var userInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(DataLogin);

        if (userInfo != null)
        {
            // Extract 'sub' as password
            string password = userInfo.ContainsKey("sub") ? userInfo["sub"] : string.Empty;

            // Extract 'email' as username
            string username = userInfo.ContainsKey("email") ? userInfo["email"] : string.Empty;

            //// Output or process username and password as needed
            output($"username: {username}");
            output($"password: {password}");
            if (!CheckCustomerByGet(username, password))
            {
                PostCustomer(username, password);
                var pageKey = typeof(Guide1ViewModel).FullName;
                if (pageKey != null)
                {
                    _navigationService.NavigateTo(pageKey, username);
                }
            }
            else
            {
                if (CheckCustomerHealthByUsername(username))
                {
                    var frame = new Frame();
                    _shell = App.GetService<ShellPage>();
                    frame.Content = _shell;
                    App.MainWindow.Content = frame;
                    await _localSettingsService.SaveSettingAsync("customer_id", customer.customer_id);
                    _navigationService.NavigateTo(typeof(HomeViewModel).FullName!);
                }
                else
                {
                    var pageKey = typeof(Guide1ViewModel).FullName;
                    if (pageKey != null)
                    {
                        _navigationService.NavigateTo(pageKey, username);
                    }
                }
            }
        }
        else
        {
            output("Failed to parse user info.");
        }
    }

    /// \brief Kiểm tra khách hàng tồn tại bằng tên đăng nhập và mật khẩu.
    /// \param username Tên đăng nhập.
    /// \param password Mật khẩu.
    /// \return True nếu khách hàng tồn tại, ngược lại false.
    private bool CheckCustomerByGet(string username, string password)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7141/");
        var response = client.GetAsync("api/Customer/get/username/" + username).Result;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var json = response.Content.ReadAsStringAsync().Result;
            customer = JsonConvert.DeserializeObject<Customer>(json);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// \brief Kiểm tra thông sức khỏe khách hàng tồn tại bằng tên đăng nhập.
    /// \param username Tên đăng nhập.
    /// \return True nếu dữ liệu sức khỏe khách hàng tồn tại, ngược lại false
    private bool CheckCustomerHealthByUsername(string username)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7141/");

        // Step 1: Get customer_id from customer table based on username
        var customerResponse = client.GetAsync("api/Customer/get/username/" + username).Result;
        if (customerResponse.StatusCode != HttpStatusCode.OK)
        {
            return false; // Customer not found
        }

        var customerJson = customerResponse.Content.ReadAsStringAsync().Result;
        var customer = JsonConvert.DeserializeObject<Customer>(customerJson);
        if (customer == null)
        {
            return false; // Customer not found
        }

        // Step 2: Check if customer_id exists in customer_health table
        var healthResponse = client.GetAsync("api/Customerhealth/get/" + customer.customer_id).Result;
        if (healthResponse.StatusCode == HttpStatusCode.OK)
        {
            return true; // Customer health data exists
        }
        else
        {
            return false; // Customer health data does not exist
        }
    }

    /// \brief Post thông tin khách hàng lên csdl qua API.
    /// \param username Tên đăng nhập.
    /// \param password Mật khẩu.
    public void PostCustomer(string username, string password)
    {
        if (customer == null)
        {
            customer = new Customer();
        }
        customer.customer_name = username;
        customer.auth_type = 2;
        customer.username = username;
        customer.customer_password = password;
        customer.role_user = 1;

        var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7141/");
        var json = System.Text.Json.JsonSerializer.Serialize(customer);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = client.PostAsync("api/Customer/create", content).Result;

    }

    /// \brief Tạo chuỗi base64 URL-encoded ngẫu nhiên.
    /// \param length Độ dài của chuỗi.
    /// \return Chuỗi đã được tạo.
    private static string randomDataBase64url(uint length)
    {
        var buffer = Windows.Security.Cryptography.CryptographicBuffer.GenerateRandom(length);
        return base64urlencodeNoPadding(buffer);
    }

    /// \brief Tính toán hash SHA-256 của chuỗi đầu vào.
    /// \param input Chuỗi đầu vào.
    /// \return Hash dưới dạng IBuffer.
    private static IBuffer sha256(string input)
    {
        var provider = Windows.Security.Cryptography.Core.HashAlgorithmProvider.OpenAlgorithm("SHA256");
        var buffer = Windows.Security.Cryptography.CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
        return provider.HashData(buffer);
    }

    /// \brief Mã hóa buffer thành chuỗi base64 URL-encoded không có padding.
    /// \param buffer Buffer để mã hóa.
    /// \return Chuỗi đã được mã hóa.
    public static string base64urlencodeNoPadding(IBuffer buffer)
    {
        string base64 = CryptographicBuffer.EncodeToBase64String(buffer);
        base64 = base64.Replace("+", "-").Replace("/", "_").Replace("=", "");
        return base64;
    }

    /// \brief Xử lý yêu cầu quên mật khẩu.
    private async void OnForgotPasswordProcessing()
    {
        var (email, username) = await _dialogService.ShowUsernameInputDialog();
        if (!string.IsNullOrEmpty(email))
        {
            Random random = new Random();
            string verificationCode = random.Next(0, 10000).ToString("D4");//Tạo mã xác thực random
            sendVerificationCode(email, verificationCode);
            if (await _dialogService.ShowVerificationDialogAsync(email, verificationCode) == true)
            {// xử lí update
                var newPassword = await _dialogService.ShowResetPasswordDialogAsync();
                if (!String.IsNullOrEmpty(newPassword))
                {
                    var endpoint = $"api/Customer/update/" + username + "/password/" + newPassword;
                    var result = ApiServices.Put<Customer>(endpoint, null);
                    if (result != null)
                    {
                        Debug.WriteLine("PUT request successful!");
                        output("Đổi mật khẩu thành công!");
                        StatusColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 128, 0));
                    }
                    else
                    {
                        Debug.WriteLine("PUT request failed.");
                        output("Đổi mật khẩu thất bại!");
                    }
                }
                else
                {
                    // do nothing
                }
            }
            else
            {
                // do nothing
            }
        }
    }

    /// \brief Gửi mã xác minh đến email được chỉ định.
    /// \param email Địa chỉ email.
    /// \param code Mã xác minh
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

    /// \brief Xuất thông báo.
    /// \param message Thông báo cần xuất.
    public void output(string message)
    {
        LoginStatus = message;
        StatusColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
        Debug.WriteLine(message);
    }
}
