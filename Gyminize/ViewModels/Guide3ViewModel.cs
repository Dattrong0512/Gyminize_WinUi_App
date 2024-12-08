using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;
using Gyminize.Models;
using Microsoft.UI.Xaml.Media;
using Gyminize.Helpers;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Net;
using Gyminize.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using Gyminize.Services;


namespace Gyminize.ViewModels;

/// \class Guide3ViewModel
/// \brief ViewModel cho màn hình hướng dẫn cuối cùng trong ứng dụng.
/// 
/// ViewModel này chịu trách nhiệm quản lý logic giao diện và dữ liệu người dùng cho màn hình Guide1.
/// Nó cung cấp các lệnh và thuộc tính được liên kết với UI.
public class Guide3ViewModel : ObservableRecipient, INavigationAware
{
    private CustomerInfo _customerInfo;
    private readonly INavigationService _navigationService;
    private readonly IWindowService _windowService;
    private double _bmi;
    private UIElement? _shell = null;
    private int customerId;
    public ILocalSettingsService _localsetting;
    private CustomerHealth customerHealth
    {
        get; set;
    }
    public double BMIStat
    {
        get => _bmi;
        set => SetProperty(ref _bmi, value);
    }
    private double _tdde;
    public double TDEEStat
    {
        get => _tdde;
        set => SetProperty(ref _tdde, value);
    }

    private double _cuttingProteins;
    public double CuttingProteins
    {
        get => _cuttingProteins;
        set => SetProperty(ref _cuttingProteins, value);
    }

    private double _cuttingCarbs;
    public double CuttingCarbs
    {
        get => _cuttingCarbs;
        set => SetProperty(ref _cuttingCarbs, value);
    }

    private double _cuttingFats;
    public double CuttingFats
    {
        get => _cuttingFats;
        set => SetProperty(ref _cuttingFats, value);
    }

    private double _maintenanceProteins;
    public double MaintenanceProteins
    {
        get => _maintenanceProteins;
        set => SetProperty(ref _maintenanceProteins, value);
    }

    private double _maintenanceCarbs;
    public double MaintenanceCarbs
    {
        get => _maintenanceCarbs;
        set => SetProperty(ref _maintenanceCarbs, value);
    }

    private double _maintenanceFats;
    public double MaintenanceFats
    {
        get => _maintenanceFats;
        set => SetProperty(ref _maintenanceFats, value);
    }

    private double _bulkingProteins;
    public double BulkingProteins
    {
        get => _bulkingProteins;
        set => SetProperty(ref _bulkingProteins, value);
    }

    private double _bulkingCarbs;
    public double BulkingCarbs
    {
        get => _bulkingCarbs;
        set => SetProperty(ref _bulkingCarbs, value);
    }

    private double _bulkingFats;

    public double BulkingFats
    {
        get => _bulkingFats;
        set => SetProperty(ref _bulkingFats, value);
    }

    private string _healthStatus;
    public string HealthStatus
    {
        get => _healthStatus;
        set => SetProperty(ref _healthStatus, value);
    }
    public ICommand NavigateNextCommand
    {
        get;
    }
    public ICommand NavigateBackCommand
    {
        get;
    }

    /// <summary>
    /// Khởi tạo một phiên bản mới của Guide3ViewModel, thiết lập dịch vụ điều hướng, cửa sổ và cài đặt.
    /// </summary>
    /// <param name="navigationService">Dịch vụ điều hướng để điều hướng giữa các trang.</param>
    /// <param name="windowService">Dịch vụ cửa sổ để thiết lập các thuộc tính của cửa sổ ứng dụng.</param>
    /// <param name="localsetting">Dịch vụ cài đặt địa phương để lưu trữ các thông tin của khách hàng.</param>
    public Guide3ViewModel(INavigationService navigationService, IWindowService windowService, ILocalSettingsService localsetting)
    {
        _windowService = windowService;
        _navigationService = navigationService;
        customerHealth = new CustomerHealth();
        NavigateBackCommand = new RelayCommand(NavigateBack);
        NavigateNextCommand = new RelayCommand(NavigateNext);
        _windowService.SetWindowSize(1200, 900);
        _windowService.SetIsMaximizable(false);
        _windowService.SetIsResizable(false);
        _localsetting = localsetting;
    }


    /// <summary>
    /// Phương thức được gọi khi trang được điều hướng đến, nhận tham số là thông tin khách hàng.
    /// </summary>
    /// <param name="parameter">Thông tin khách hàng (CustomerInfo) được truyền vào.</param>
    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is CustomerInfo customerInfo)
        {
            customerId = GetCustomerIdByUsername(customerInfo.username);
            await SaveLocalSettings(customerId);
            _customerInfo = customerInfo;
            TDEEStat = Math.Round(HealthCalculator.CalculateTDEE(_customerInfo.Weight, _customerInfo.BodyFat, _customerInfo.ActivityLevel), 0);
            BMIStat = Math.Round(HealthCalculator.CalculateBMI(_customerInfo.Weight, _customerInfo.Height), 1);
            _customerInfo.Tdee = (int)TDEEStat;
            MaintenanceProteins = Math.Round(HealthCalculator.CalculateNutritionGram("proteins", "m", 30, _tdde), 0);
            MaintenanceCarbs = Math.Round(HealthCalculator.CalculateNutritionGram("carbs", "m", 35, _tdde), 0);
            MaintenanceFats = Math.Round(HealthCalculator.CalculateNutritionGram("fats", "m", 35, _tdde), 0);
            BulkingProteins = Math.Round(HealthCalculator.CalculateNutritionGram("proteins", "b", 30, _tdde), 0);
            BulkingCarbs = Math.Round(HealthCalculator.CalculateNutritionGram("carbs", "b", 35, _tdde), 0);
            BulkingFats = Math.Round(HealthCalculator.CalculateNutritionGram("fats", "b", 35, _tdde), 0);
            CuttingProteins = Math.Round(HealthCalculator.CalculateNutritionGram("proteins", "c", 30, _tdde), 0);
            CuttingCarbs = Math.Round(HealthCalculator.CalculateNutritionGram("carbs", "c", 35, _tdde), 0);
            CuttingFats = Math.Round(HealthCalculator.CalculateNutritionGram("fats", "c", 35, _tdde), 0);
            HealthStatus = HealthCalculator.HealthStatus(BMIStat);
        }
    }

    /// <summary>
    /// Phương thức được gọi khi điều hướng từ trang này.
    /// </summary>
    public void OnNavigatedFrom()
    {
        // Không thực hiện gì ở đây
    }

    /// <summary>
    /// Phương thức nhận mã khách hàng từ tên người dùng.
    /// </summary>
    /// <param name="username">Tên người dùng của khách hàng.</param>
    /// <returns>Trả về mã khách hàng nếu tìm thấy, -1 nếu không tìm thấy khách hàng.</returns>
    private int GetCustomerIdByUsername(string username)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7141/");
        var response = client.GetAsync("api/Customer/get/username/" + username).Result;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var json = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<Customer>(json);
            return customer.customer_id;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// Thêm thông tin sức khỏe của khách hàng vào cơ sở dữ liệu.
    /// </summary>
    /// <param name="customerInfo">Thông tin khách hàng.</param>
    /// <param name="customerId">Mã khách hàng.</param>
    /// <returns>Trả về true nếu thành công, false nếu thất bại.</returns>
    private bool InsertCustomerHealth(CustomerInfo customerInfo, int customerId)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7141/");

        try
        {
            customerHealth.customer_id = customerId;
            customerHealth.gender = customerInfo.sex;
            customerHealth.height = customerInfo.Height;
            customerHealth.weight = customerInfo.Weight;  
            customerHealth.age = customerInfo.Age;
            customerHealth.activity_level = customerInfo.ActivityLevel;
            customerHealth.body_fat = (decimal)customerInfo.BodyFat;
            customerHealth.tdee = (decimal)customerInfo.Tdee;

            var json = JsonConvert.SerializeObject(customerHealth);
            Debug.WriteLine($"Request JSON: {json}");

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync("api/Customerhealth/create", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                Debug.WriteLine($"Error Details: {errorContent}");
            }

            return response.StatusCode == HttpStatusCode.Created;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Thêm thông tin sức khỏe của khách hàng bằng tên người dùng.
    /// </summary>
    /// <param name="customerInfo">Thông tin khách hàng.</param>
    /// <returns>Trả về true nếu thành công, false nếu thất bại.</returns>
    public bool AddCustomerHealthByUsername(CustomerInfo customerInfo)
    {
        if (customerId != -1)
        {
            return InsertCustomerHealth(customerInfo, customerId);
        }
        else
        {
            return false; // Customer not found
        }
    }

    /// <summary>
    /// Lưu mã khách hàng vào cài đặt địa phương.
    /// </summary>
    /// <param name="customerId">Mã khách hàng.</param>
    /// <returns>Trả về một tác vụ bất đồng bộ khi lưu xong.</returns>
    private async Task SaveLocalSettings(int customerId)
    {
        await _localsetting.SaveSettingAsync("customer_id", customerId.ToString());
        var testValue = await _localsetting.ReadSettingAsync<string>("customer_id");
        Console.WriteLine("Giá trị vừa lưu là: " + testValue);
    }

    /// <summary>
    /// Phương thức điều hướng về trang trước (Guide2ViewModel).
    /// </summary>
    private void NavigateBack()
    {
        var pageKey = typeof(Guide2ViewModel).FullName;
        if (pageKey != null)
        {
            _navigationService.NavigateTo(pageKey, _customerInfo);
        }
    }

    /// <summary>
    /// Phương thức điều hướng tới trang tiếp theo (HomeViewModel).
    /// </summary>
    private void NavigateNext()
    {
        _ = AddCustomerHealthByUsername(_customerInfo);

        if (App.MainWindow.Content != null)
        {
            var frame = new Frame();
            _shell = App.GetService<ShellPage>();
            frame.Content = _shell;
            App.MainWindow.Content = frame;
            _navigationService.NavigateTo(typeof(HomeViewModel).FullName!);
        }
    }
}

