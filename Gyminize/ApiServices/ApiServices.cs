﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gyminize.APIServices
{
    public static class ApiServices
    {
        private static readonly HttpClient _client;

        static ApiServices()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7141/"); // Thay đổi base URL nếu cần
        }

        // Hàm POST tổng quát dạng static
        public static T Post<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _client.PostAsync(endpoint, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    // Xử lý lỗi
                    var errorContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    Debug.WriteLine($"Error Details: {errorContent}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return default;
            }
        }

        // Hàm GET tổng quát dạng static
        public static T Get<T>(string endpoint)
        {
            try
            {
                var response = _client.GetAsync(endpoint).Result;

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    // Xử lý lỗi
                    var errorContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    Debug.WriteLine($"Error Details: {errorContent}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                return default;
            }
        }
    }
}
