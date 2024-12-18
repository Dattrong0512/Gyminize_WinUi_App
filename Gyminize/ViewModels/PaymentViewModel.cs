// ViewModel cho trang cửa hàng.
// Kế thừa từ ObservableRecipient để hỗ trợ thông báo thay đổi thuộc tính.
// Thực hiện giao diện INavigationAware để nhận biết điều hướng.
using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Models;
using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;
using Gyminize.Core.Contracts.Services;
using Gyminize.Views;
using System.Diagnostics;
using Microsoft.UI.Xaml;


namespace Gyminize.ViewModels;

/// <summary>
/// ViewModel cho màn hình thanh toán của ứng dụng.
/// </summary>
/// <remarks>
/// Lớp này chịu trách nhiệm quản lý các thao tác liên quan đến thanh toán, 
/// bao gồm việc chỉnh sửa thông tin người dùng (tên, số điện thoại, địa chỉ), 
/// chọn phương thức thanh toán, và xử lý quá trình thanh toán.
/// </remarks>
public partial class PaymentViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly ILocalSettingsService _localSettingsService;
    private readonly IDialogService _dialogService;
    private readonly IApiServicesClient _apiServicesClient;

    private decimal _totalPrice;
    public decimal TotalPrice
    {
        get => _totalPrice;
        set => SetProperty(ref _totalPrice, value);
    }

    public bool CanProceedToPayment => !IsNameInvalid && !IsPhoneNumberInvalid && !IsAddressInvalid &&
                   !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(PhoneNumber) && !string.IsNullOrWhiteSpace(Address) &&
                   (IsMomoSelected || IsBankSelected || IsCodSelected) && (IsNameEnabled == false || IsPhoneNumberEnabled == false || IsAddressEnabled == false ) ;


    private decimal _shippingFee;
    public decimal ShippingFee
    {
        get => _shippingFee;
        set => SetProperty(ref _shippingFee, value);
    }

    private decimal _productPrice;
    public decimal ProductPrice
    {
        get => _productPrice;
        set => SetProperty(ref _productPrice, value);
    }


    private bool _isMomoSelected;
    public bool IsMomoSelected
    {
        get => _isMomoSelected;
        set => SetProperty(ref _isMomoSelected, value);
    }

    private bool _isBankSelected;
    public bool IsBankSelected
    {
        get => _isBankSelected;
        set => SetProperty(ref _isBankSelected, value);
    }

    private bool _isCodSelected;
    public bool IsCodSelected
    {
        get => _isCodSelected;
        set => SetProperty(ref _isCodSelected, value);
    }

    private string _name;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _phoneNumber;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    private string _address;
    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    private bool _isNameInvalid;
    public bool IsNameInvalid
    {
        get => _isNameInvalid;
        set => SetProperty(ref _isNameInvalid, value);
    }

    private bool _isPhoneNumberInvalid;
    public bool IsPhoneNumberInvalid
    {
        get => _isPhoneNumberInvalid;
        set => SetProperty(ref _isPhoneNumberInvalid, value);
    }

    private bool _isAddressInvalid;
    public bool IsAddressInvalid
    {
        get => _isAddressInvalid;
        set => SetProperty(ref _isAddressInvalid, value);
    }

    private bool _isNameEnabled;
    public bool IsNameEnabled
    {
        get => _isNameEnabled;
        set => SetProperty(ref _isNameEnabled, value);
    }

    private bool _isPhoneNumberEnabled;
    public bool IsPhoneNumberEnabled
    {
        get => _isPhoneNumberEnabled;
        set => SetProperty(ref _isPhoneNumberEnabled, value);
    }

    private bool _isAddressEnabled;
    public bool IsAddressEnabled
    {
        get => _isAddressEnabled;
        set => SetProperty(ref _isAddressEnabled, value);
    }

    public Orders currentOrder = new();
    public ICommand TogglePaymentMethod { get; }
    public ICommand EditNameCommand { get; }
    public ICommand SaveNameCommand { get; }
    public ICommand EditPhoneNumberCommand { get; }
    public ICommand SavePhoneNumberCommand { get; }
    public ICommand EditAddressCommand { get; }
    public ICommand SaveAddressCommand { get; }

    public ICommand PaymentCommand
    {
    get;
    }

    /// <summary>
    /// Khởi tạo ViewModel cho màn hình thanh toán.
    /// </summary>
    /// <param name="navigationService">Dịch vụ điều hướng giữa các trang.</param>
    /// <param name="localSettingsService">Dịch vụ lưu trữ cài đặt người dùng.</param>
    /// <param name="dialogService">Dịch vụ hiển thị hộp thoại.</param>
    /// <param name="apiServicesClient">Dịch vụ kết nối API.</param>
    public PaymentViewModel(INavigationService navigationService, ILocalSettingsService localSettingsService, IDialogService dialogService, IApiServicesClient apiServicesClient)
    {
        _navigationService = navigationService;
        _localSettingsService = localSettingsService;
        _dialogService = dialogService;
        _apiServicesClient = apiServicesClient;

        TogglePaymentMethod = new RelayCommand<string>(TogglePaymentMethodExecute);
        EditNameCommand = new RelayCommand(EditName);
        SaveNameCommand = new RelayCommand(SaveName);
        EditPhoneNumberCommand = new RelayCommand(EditPhoneNumber);
        SavePhoneNumberCommand = new RelayCommand(SavePhoneNumber);
        EditAddressCommand = new RelayCommand(EditAddress);
        SaveAddressCommand = new RelayCommand(SaveAddress);
        PaymentCommand = new RelayCommand(PaymentProcess);

        LoadOrderDetailData();
    }

    /// <summary>
    /// Kiểm tra xem có thể tiếp tục thanh toán hay không.
    /// </summary>
    private void CheckCanProceedToPayment()
    {
        OnPropertyChanged(nameof(CanProceedToPayment));
    }

    /// <summary>
    /// Thực hiện thao tác khi người dùng chọn phương thức thanh toán.
    /// </summary>
    /// <param name="method">Phương thức thanh toán (Momo, Ngân hàng, COD).</param>
    private void TogglePaymentMethodExecute(string method)
    {
        switch (method)
        {
            case "Momo":
                IsMomoSelected = true;
                IsBankSelected = false;
                IsCodSelected = false;
                break;
            case "Bank":
                IsMomoSelected = false;
                IsBankSelected = true;
                IsCodSelected = false;
                break;
            case "Cod":
                IsMomoSelected = false;
                IsBankSelected = false;
                IsCodSelected = true;
                break;
        }
        CheckCanProceedToPayment();
    }

    /// <summary>
    /// Tải chi tiết đơn hàng của khách hàng.
    /// </summary>
    public async void LoadOrderDetailData()
    {
        await getCurrentOrder();
        var endpoint = $"api/Customer/get/" + currentOrder.customer_id;
        var customer = _apiServicesClient.Get<Customer>(endpoint);

        Name = customer.customer_name ?? "";

        PhoneNumber = currentOrder.phone_number;
        Address = currentOrder.address;

        _productPrice = currentOrder.total_price;
        _shippingFee = 0;

        _totalPrice = _productPrice + _shippingFee;
    }

    /// <summary>
    /// Kiểm tra chuỗi có chứa ký tự đặc biệt hay không.
    /// </summary>
    /// <param name="input">Chuỗi cần kiểm tra.</param>
    /// <returns>True nếu có ký tự đặc biệt, False nếu không.</returns>
    private bool ContainsSpecialCharacters(string input)
    {
        string pattern = @"[^a-zA-Z0-9\sÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]";
        return System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
    }

    /// <summary>
    /// Cho phép người dùng chỉnh sửa tên.
    /// </summary>
    private void EditName()
    {
        IsNameEnabled = true;
    }

    /// <summary>
    /// Lưu tên người dùng sau khi chỉnh sửa.
    /// </summary>
    private void SaveName()
    {
        if (string.IsNullOrEmpty(Name))
        {
            IsNameInvalid = true;
        }
        else
        {
            IsNameInvalid = false;
            IsNameEnabled = false;
        }
        CheckCanProceedToPayment();
    }

    /// <summary>
    /// Cho phép người dùng chỉnh sửa số điện thoại.
    /// </summary>
    private void EditPhoneNumber()
    {
        IsPhoneNumberEnabled = true;
    }

    /// <summary>
    /// Lưu số điện thoại người dùng sau khi chỉnh sửa.
    /// </summary>
    private void SavePhoneNumber()
    {
        if (string.IsNullOrEmpty(PhoneNumber) || PhoneNumber.Length != 10 || !PhoneNumber.All(char.IsDigit))
        {
            IsPhoneNumberInvalid = true;
        }
        else
        {
            var resultPayment = _apiServicesClient.Put<Orders>($"api/Order/update/orders_id/{currentOrder.orders_id}/phone_number/{PhoneNumber}", null);
            if(resultPayment == null)
            {
                IsPhoneNumberInvalid = false;
                IsPhoneNumberEnabled = false;
                _dialogService.ShowSuccessMessageAsync("Cập nhật số điện thoại thành công");
            };
        }
        CheckCanProceedToPayment();
    }

    /// <summary>
    /// Cho phép người dùng chỉnh sửa địa chỉ.
    /// </summary>
    private void EditAddress()
    {
        IsAddressEnabled = true;
    }

    /// <summary>
    /// Lưu địa chỉ người dùng sau khi chỉnh sửa.
    /// </summary>
    private void SaveAddress()
    {
        if (string.IsNullOrEmpty(Address))
        {
            IsAddressInvalid = true;
        }
        else
        {
            var resultPayment = _apiServicesClient.Put<Orders>($"api/Order/update/orders_id/{currentOrder.orders_id}/address/{Address}", null);
            if (resultPayment == null)
            {
                IsAddressInvalid = false;
                IsAddressEnabled = false;
                _dialogService.ShowSuccessMessageAsync("Cập nhật địa chỉ thành công");
            }
        }
        CheckCanProceedToPayment();
    }

    /// <summary>
    /// Lấy thông tin đơn hàng hiện tại từ cài đặt.
    /// </summary>
    public async Task getCurrentOrder()
    {
        currentOrder = await _localSettingsService.ReadSettingAsync<Orders>("currentOrder");
    }

    /// <summary>
    /// Xử lý quá trình thanh toán.
    /// </summary>
    private async void PaymentProcess()
    {
        if (IsMomoSelected)
        {
            // Xử lý thanh toán qua Momo
        }
        else if (IsBankSelected)
        {
            var newPayment = new Payment();
            newPayment.payment_amount = TotalPrice;
            
            var resultPayment = _apiServicesClient.Post<Payment>($"api/Cart/createPaymentVnpay/orderId/" + currentOrder.orders_id, newPayment);
            if (resultPayment != null)
            {
                var status = await _dialogService.ShowVNPAYPaymentProcessDialogAsync(currentOrder.orders_id);
                if (status <= 0)
                {
                    //do nothing
                }
                else if (status == 1)
                {
                    var pageKey = typeof(ShopViewModel).FullName;
                    if (pageKey != null)
                    {
                        _navigationService.NavigateTo(pageKey);
                    }
                }
            }
        }
        else if (IsCodSelected)
        {
            var resultPayment = _apiServicesClient.Put<Orders>($"api/Order/update/orders_id/{currentOrder.orders_id}/status/Completed-COD", null);
            if(resultPayment == null)
            {
                await _dialogService.ShowSuccessMessageAsync("Đặt hàng thành công");
                var pageKey = typeof(ShopViewModel).FullName;
                if (pageKey != null)
                {
                    _navigationService.NavigateTo(pageKey);
                }
            } 
        }
    }
}








