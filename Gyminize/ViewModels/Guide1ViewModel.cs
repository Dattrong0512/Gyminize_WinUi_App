using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Gyminize.Contracts.Services;
using Gyminize.Views;
using Gyminize.Models;
using Microsoft.UI.Xaml.Media;
using Windows.Gaming.Input.ForceFeedback;
using Gyminize.Contracts.ViewModels;
using Windows.ApplicationModel.Email;
namespace Gyminize.ViewModels;

/// \class Guide1ViewModel
/// \brief ViewModel cho màn hình hướng dẫn đầu tiên trong ứng dụng.
/// 
/// ViewModel này chịu trách nhiệm quản lý logic giao diện và dữ liệu người dùng cho màn hình Guide1.
/// Nó cung cấp các lệnh và thuộc tính được liên kết với UI.
public partial class Guide1ViewModel : ObservableRecipient, INavigationAware
{
    private string _username;
    private CustomerInfo _customerInfoBack;
    private readonly INavigationService _navigationService;
    private readonly IWindowService _windowService;

    /// \brief Khởi tạo một đối tượng Guide1ViewModel.
    /// \param navigationService Dịch vụ điều hướng được sử dụng để chuyển giữa các trang.
    /// \param windowService Dịch vụ quản lý cửa sổ ứng dụng.
    public Guide1ViewModel(INavigationService navigationService, IWindowService windowService)
    {
        _windowService = windowService;
        _navigationService = navigationService;
        MaleCheckCommand = new RelayCommand<RoutedEventArgs?>(MaleSexCheck);
        FemaleCheckCommand = new RelayCommand<RoutedEventArgs?>(FemaleSexCheck);    
        AgeLostFocusCommand = new RelayCommand<RoutedEventArgs?>(OnAgeLostFocus);
        HeightLostFocusCommand = new RelayCommand<RoutedEventArgs?>(OnHeightLostFocus);
        WeightLostFocusCommand = new RelayCommand<RoutedEventArgs?>(OnWeightLostFocus);
        NavigateToGuidePage2Command = new RelayCommand(NavigateToGuidePage2);

        _maleCheckBox = new CheckBox();
        _femaleCheckBox = new CheckBox();
        _ageTextBox = new TextBox();
        _ageErrorTextBlock = new TextBlock();
        _heightTextBox = new TextBox();
        _heightErrorTextBlock = new TextBlock();
        _weightTextBox = new TextBox();
        _weightErrorTextBlock = new TextBlock();
        _customerInfoBack = new CustomerInfo();
        _activityLevelComboBox = new ComboBox();

        SelectedActivityLevel = new ComboBoxItem { Content = "Trung Bình ( 3 - 5 buổi/tuần )" };
        _windowService.SetIsResizable(false);
        _windowService.SetIsMaximizable(false);
        _windowService.SetWindowSize(1200, 800);
    }

    /// \brief Lệnh xử lý khi chọn giới tính nam.
    public ICommand MaleCheckCommand { get; }
    /// \brief Lệnh xử lý khi chọn giới tính nữ.
    public ICommand FemaleCheckCommand{ get;}
    /// \brief Lệnh xử lý khi mất focus khỏi ô nhập tuổi.
    public ICommand AgeLostFocusCommand { get; }
    /// \brief Lệnh xử lý khi mất focus khỏi ô nhập chiều cao.
    public ICommand HeightLostFocusCommand { get; }
    /// \brief Lệnh xử lý khi mất focus khỏi ô nhập cân nặng.
    public ICommand WeightLostFocusCommand { get; }
    /// \brief Lệnh chuyển sang trang hướng dẫn thứ 2.
    public ICommand NavigateToGuidePage2Command{ get; }


    private ComboBox _activityLevelComboBox;
    public ComboBox ActivityLevelComboBox
    {
        get => _activityLevelComboBox;
        set => SetProperty(ref _activityLevelComboBox, value);
    }

    private ComboBoxItem _selectedActivityLevel;
    public ComboBoxItem SelectedActivityLevel
    {
        get => _selectedActivityLevel;
        set => SetProperty(ref _selectedActivityLevel, value);
    }
    private CheckBox _maleCheckBox;
    public CheckBox MaleCheckBox
    {
        get => _maleCheckBox;
        set => SetProperty(ref _maleCheckBox, value);
    }

    private CheckBox _femaleCheckBox;
    public CheckBox FemaleCheckBox
    {
        get => _femaleCheckBox;
        set => SetProperty(ref _femaleCheckBox, value);
    }

    private TextBox _ageTextBox;
    public TextBox AgeTextBox
    {
        get => _ageTextBox;
        set => SetProperty(ref _ageTextBox, value);
    }

    private TextBlock _ageErrorTextBlock;
    public TextBlock AgeErrorTextBlock
    {
        get => _ageErrorTextBlock;
        set => SetProperty(ref _ageErrorTextBlock, value);
    }

    private TextBox _heightTextBox;
    public TextBox HeightTextBox
    {
        get => _heightTextBox;
        set => SetProperty(ref _heightTextBox, value);
    }

    private TextBlock _heightErrorTextBlock;
    public TextBlock HeightErrorTextBlock
    {
        get => _heightErrorTextBlock;
        set => SetProperty(ref _heightErrorTextBlock, value);
    }

    private TextBox _weightTextBox;
    public TextBox WeightTextBox
    {
        get => _weightTextBox;
        set => SetProperty(ref _weightTextBox, value);
    }

    private TextBlock _weightErrorTextBlock;
    public TextBlock WeightErrorTextBlock
    {
        get => _weightErrorTextBlock;
        set => SetProperty(ref _weightErrorTextBlock, value);
    }

    /// \brief Xử lý sự kiện khi người dùng chọn giới tính Nam.
    /// \param e Đối tượng RoutedEventArgs chứa thông tin sự kiện.
    /// \details Hàm này đảm bảo chỉ có một trong hai giới tính (Nam hoặc Nữ) được chọn tại một thời điểm.
    private void MaleSexCheck(RoutedEventArgs? e)
    {
        
        if (FemaleCheckBox.IsChecked == true)
        {
            FemaleCheckBox.IsChecked = false;
        }
        
    }

    /// \brief Xử lý sự kiện khi người dùng chọn giới tính Nữ.
    /// \param e Đối tượng RoutedEventArgs chứa thông tin sự kiện.
    /// \details Hàm này đảm bảo chỉ có một trong hai giới tính (Nam hoặc Nữ) được chọn tại một thời điểm.
    private void FemaleSexCheck(RoutedEventArgs? e)
    {

        if (MaleCheckBox.IsChecked == true)
        {
            MaleCheckBox.IsChecked = false;
        }

    }

    /// \brief Xử lý sự kiện khi mất tiêu điểm tại ô nhập tuổi.
    /// \param e Đối tượng RoutedEventArgs chứa thông tin về sự kiện.
    /// \details Hàm này kiểm tra độ tuổi nhập vào có hợp lệ hay không (trong khoảng từ 16 đến 70).
    private void OnAgeLostFocus(RoutedEventArgs? e)
    {
        if (int.TryParse(AgeTextBox.Text, out int age))
        {
            if (age < 16 || age > 70)
            {
                AgeErrorTextBlock.Visibility = Visibility.Visible;
                AgeErrorTextBlock.Text = "*Ứng dụng chỉ hỗ trợ đối tượng từ 16-70 tuổi";
              
            }
            else
            {
                AgeErrorTextBlock.Visibility = Visibility.Collapsed;
                AgeErrorTextBlock.Text = "*OK";
 
            }
        }
        else if (string.IsNullOrEmpty(AgeTextBox.Text))
        {
            AgeErrorTextBlock.Visibility = Visibility.Visible;
            AgeErrorTextBlock.Text = "*Vui lòng nhập một độ tuổi hợp lệ";

        }
    }

    /// \brief Xử lý sự kiện khi mất tiêu điểm tại ô nhập chiều cao.
    /// \param e Đối tượng RoutedEventArgs chứa thông tin về sự kiện.
    /// \details Hàm này kiểm tra chiều cao nhập vào có hợp lệ hay không (trong khoảng từ 110 đến 210 cm).
    private void OnHeightLostFocus(RoutedEventArgs? e)
    {
        if (int.TryParse(HeightTextBox.Text, out int height))
        {
            if (height < 110 || height > 210)
            {
                HeightErrorTextBlock.Visibility = Visibility.Visible;
                HeightErrorTextBlock.Text = "*Chiều cao không được hỗ trợ";
            }
            else
            {
                HeightErrorTextBlock.Visibility = Visibility.Collapsed;
                HeightErrorTextBlock.Text = "*OK";
            }
        }
        else if (string.IsNullOrEmpty(HeightTextBox.Text))
        {
            HeightErrorTextBlock.Visibility = Visibility.Visible;
            HeightErrorTextBlock.Text = "*Vui lòng nhập một chiều cao hợp lệ";
        }
    }

    /// \brief Xử lý sự kiện khi mất tiêu điểm tại ô nhập cân nặng.
    /// \param e Đối tượng RoutedEventArgs chứa thông tin về sự kiện.
    /// \details Hàm này kiểm tra cân nặng nhập vào có hợp lệ hay không (trong khoảng từ 35 đến 250 kg).
    private void OnWeightLostFocus(RoutedEventArgs? e)
    {
        if (double.TryParse(WeightTextBox.Text, out var weight))
        {
            if (weight < 35 || weight > 250)
            {
                WeightErrorTextBlock.Visibility = Visibility.Visible;
                WeightErrorTextBlock.Text = "*Cân nặng không được hỗ trợ";
            }
            else
            {
                WeightErrorTextBlock.Visibility = Visibility.Collapsed;
                WeightErrorTextBlock.Text = "*OK";
            }
        }
        else if (string.IsNullOrEmpty(WeightTextBox.Text))
        {
            WeightErrorTextBlock.Visibility = Visibility.Visible;
            WeightErrorTextBlock.Text = "*Vui lòng nhập một cân nặng hợp lệ";
        }
    }

    /// \brief Lấy mức độ hoạt động đã chọn từ ComboBox.
    /// \return Giá trị mức độ hoạt động (1: Không vận động, 2: Thấp, 3: Trung bình, 4: Cao).
    /// \details Hàm này trả về giá trị của mức độ hoạt động mà người dùng đã chọn trong ComboBox.
    private int GetSelectedActivityLevel()
    {
        return SelectedActivityLevel?.Content switch
        {
            "Hầu như không vận động" => 1,
            "Thấp ( 1 - 2 buổi/tuần )" => 2,
            "Trung Bình ( 3 - 5 buổi/tuần )" => 3,
            "Cao ( 6 - 7 buổi/tuần )" => 4,
            _ => 0
        };
    }

    /// \brief Lấy tên mức độ hoạt động từ giá trị.
    /// \param activityLevel Giá trị mức độ hoạt động (1-4).
    /// \return Tên mức độ hoạt động tương ứng.
    /// \details Hàm này trả về tên mức độ hoạt động dựa trên giá trị được cung cấp.
    private string GetActivityLevel(int activityLevel)
    {
        return activityLevel switch
        {
            1 => "Hầu như không vận động",
            2 => "Thấp ( 1 - 2 buổi/tuần )",
            3 => "Trung Bình ( 3 - 5 buổi/tuần )",
            4 => "Cao ( 6 - 7 buổi/tuần )",
            _ => "Không xác định"
        };
    }

    /// \brief Điều hướng tới trang Hướng dẫn 2.
    /// \details Hàm này kiểm tra dữ liệu người dùng, nếu hợp lệ thì chuyển hướng sang trang 2 với dữ liệu người dùng.
    private void NavigateToGuidePage2()
    {
        if(isValidData()) {
            var customerInfo = new CustomerInfo
            {
                username = _username,
                sex = MaleCheckBox.IsChecked == true ? 1 : 0,
                Age = int.Parse(AgeTextBox.Text),
                Weight = int.Parse(WeightTextBox.Text),
                Height = int.Parse(HeightTextBox.Text),
                ActivityLevel = ((int)GetSelectedActivityLevel()),
                BodyFat = _customerInfoBack.BodyFat != 0 ? _customerInfoBack.BodyFat : 0

            };


            var pageKey = typeof(Guide2ViewModel).FullName;
            if (pageKey != null)
            {
                _navigationService.NavigateTo(pageKey, customerInfo);
            }
        }
        else
        {
            AgeErrorTextBlock.Visibility = Visibility.Visible;
            HeightErrorTextBlock.Visibility = Visibility.Visible;
            WeightErrorTextBlock.Visibility = Visibility.Visible;
        }
    }

    /// \brief Xử lý khi ViewModel nhận tham số điều hướng.
    /// \param parameter Đối tượng truyền vào khi điều hướng.
    public void OnNavigatedTo(object parameter)
    {
        if (parameter is CustomerInfo customerInfo)
        {
            _customerInfoBack = customerInfo;

            if (_customerInfoBack != null)
            {
                if (_customerInfoBack.sex == 1)
                {
                    MaleCheckBox.IsChecked = true;
                }
                else
                {
                    FemaleCheckBox.IsChecked = true;
                }
                WeightTextBox.Text = _customerInfoBack.Weight.ToString();
                HeightTextBox.Text = _customerInfoBack.Height.ToString();
                AgeTextBox.Text = _customerInfoBack.Age.ToString();
                SelectedActivityLevel = new ComboBoxItem { Content = GetActivityLevel(_customerInfoBack.ActivityLevel) };
                customerInfo.BodyFat = _customerInfoBack.BodyFat;
            }
        } else if(parameter is string user)
        {
            _username = user;
        }
    }

    /// \brief Kiểm tra dữ liệu người dùng có hợp lệ không.
    /// \return true nếu dữ liệu hợp lệ, ngược lại false.
    private bool isValidData()
    {
        bool isAgeValid = AgeErrorTextBlock.Visibility != Visibility.Visible && !string.IsNullOrEmpty(AgeTextBox.Text);
        bool isHeightValid = HeightErrorTextBlock.Visibility != Visibility.Visible && !string.IsNullOrEmpty(HeightTextBox.Text);
        bool isWeightValid = WeightErrorTextBlock.Visibility != Visibility.Visible && !string.IsNullOrEmpty(WeightTextBox.Text);

        return isAgeValid && isHeightValid && isWeightValid;
    }

    public void OnNavigatedFrom()
    {
    }
}
