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
namespace Gyminize.ViewModels
{
    public partial class SigninViewmodel : ObservableObject
    {
        [ObservableProperty]
        private string loginStatus;

        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;
        private Customer Customer { get; set; }
        public ICommand LoginCommandByGoogle
        {
            get;
        }
        public ICommand LoginCommandByUser
        {
            get;
        }

        // OAuth 2.0 client configuration
        const string clientID = "25264695175-026qgqsmvslrgn5gm2vj9gseu1ugf4ro.apps.googleusercontent.com";
        const string clientSecret = "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"; // Thêm client_secret
        const string redirectURI = "http://localhost:8080";  // Sử dụng URI localhost để nhận phản hồi
        const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        const string tokenEndpoint = "https://oauth2.googleapis.com/token";
        const string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";
        
        public SigninViewmodel()
        {
            LoginCommandByGoogle = new RelayCommand(OnLoginByGoogle);
            LoginCommandByUser = new RelayCommand(OnLoginByUser);
            Customer = new Customer();
        }
        private async void OnLoginByUser()
        {
            if (CheckCustomerByGet(Username, Password))
            {
               if(Password!= Customer.customer_password)
                {
                    output("Mật khẩu không đúng!");
                }
                else
                {
                    output("Đăng nhập thành công!");
                }          
            }
            else 
            {
                output("Tài khoản chưa tồn tại, vui lòng đăng kí");
            }
        }
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

        private async Task ListenForOAuthCallback()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/"); // Listening on port 8080
            listener.Start();
            //output("Listening for OAuth callback on http://localhost:8080/...");

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

        // Exchange the authorization code for tokens
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

        private void ProcessingDataLogin(string DataLogin)
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
                }
                else { //Nothing todo
                      }
            }
            else
            {
                output("Failed to parse user info.");
            }
        }
        private bool CheckCustomerByGet(string username, string password)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7141/");
            var response = client.GetAsync("api/Customer/get/username/" + username).Result;
            if (response.StatusCode==HttpStatusCode.OK)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                Customer = JsonConvert.DeserializeObject<Customer>(json);
                return true;
            }
            else
            {
                return false;             
            }
        }
        private void PostCustomer(string username, string password)
        {
            Customer.customer_name= username;
            Customer.auth_type = 2;
            Customer.username = username;
            Customer.customer_password = password;

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7141/");
            var json = System.Text.Json.JsonSerializer.Serialize(Customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync("api/Customer/create", content).Result;
        }
        // Utility methods for base64 encoding and hashing
        private static string randomDataBase64url(uint length)
        {
            var buffer = Windows.Security.Cryptography.CryptographicBuffer.GenerateRandom(length);
            return base64urlencodeNoPadding(buffer);
        }

        private static IBuffer sha256(string input)
        {
            var provider = Windows.Security.Cryptography.Core.HashAlgorithmProvider.OpenAlgorithm("SHA256");
            var buffer = Windows.Security.Cryptography.CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
            return provider.HashData(buffer);
        }

        public static string base64urlencodeNoPadding(IBuffer buffer)
        {
            string base64 = CryptographicBuffer.EncodeToBase64String(buffer);
            base64 = base64.Replace("+", "-").Replace("/", "_").Replace("=", "");
            return base64;
        }

        public void output(string message)
        {
            LoginStatus = message;
            Debug.WriteLine(message);
        }
    }
}
