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


namespace Gyminize.ViewModels;


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
                   (IsMomoSelected || IsBankSelected || IsCodSelected);


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

    private void CheckCanProceedToPayment()
    {
        OnPropertyChanged(nameof(CanProceedToPayment));
    }
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

    public async void LoadOrderDetailData()
    {
        await getCurrentOrder();
        var endpoint = $"api/Customer/get/" + currentOrder.customer_id;
        var customer = _apiServicesClient.Get<Customer>(endpoint);
        if(customer.customer_name == null)
        {
            Name = "";
        } else
        {
            Name = customer.customer_name;
        }
        PhoneNumber = currentOrder.phone_number;
        Address = currentOrder.address;
        _productPrice = currentOrder.total_price;
        _shippingFee = 20;
        _totalPrice = _productPrice + _shippingFee;
    }

    private bool ContainsSpecialCharacters(string input)
    {
        string pattern = @"[^a-zA-Z0-9\sÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]";
        return System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
    }

    private void EditName()
    {
        IsNameEnabled = true;
    }

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

    private void EditPhoneNumber()
    {
        IsPhoneNumberEnabled = true;
    }

    private void SavePhoneNumber()
    {
        if (string.IsNullOrEmpty(PhoneNumber) || PhoneNumber.Length != 11 || !PhoneNumber.All(char.IsDigit))
        {
            IsPhoneNumberInvalid = true;
        }
        else
        {
            IsPhoneNumberInvalid = false;
            IsPhoneNumberEnabled = false;
            // Lưu phone vào dtb
        }
        CheckCanProceedToPayment();
    }

    private void EditAddress()
    {
        IsAddressEnabled = true;
    }

    private void SaveAddress()
    {
        if (string.IsNullOrEmpty(Address))
        {
            IsAddressInvalid = true;
        }
        else
        {
            IsAddressInvalid = false;
            IsAddressEnabled = false;
            //lưu address vào dtb
        }
        CheckCanProceedToPayment();
    }

    public async Task getCurrentOrder()
    {
        currentOrder = await _localSettingsService.ReadSettingAsync<Orders>("currentOrder");
    }

    private void PaymentProcess()
    {
        if(IsMomoSelected == true) 
        { 
            //Xử lí thanh toán MOMO
        }
        else if (IsBankSelected == true)
        {
            //Xử lí thanh toán Ngân hàng (hoặc vnpay)
        }
        else if(IsCodSelected == true)
        {
            //Xử lí thanh toán nhận hàng
        }
    }
}
