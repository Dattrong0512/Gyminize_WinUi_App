using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gyminize.Contracts.Services;
using Gyminize.Contracts.ViewModels;
using Gyminize.Models;
using Gyminize.Services;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace Gyminize.ViewModels;

/// \class Guide2ViewModel
/// \brief ViewModel cho màn hình hướng dẫn thứ trong ứng dụng.
/// 
/// ViewModel này chịu trách nhiệm quản lý logic giao diện và dữ liệu người dùng cho màn hình Guide2.
/// Nó cung cấp các lệnh và thuộc tính được liên kết với UI.
public class Guide2ViewModel : ObservableRecipient, INavigationAware
{
    private Border? _selectedBorder;
    private CustomerInfo _customerInfo; 
    private string _imageSource1 = string.Empty; 
    private string _imageSource2 = string.Empty; 
    private string _imageSource3 = string.Empty; 
    private string _imageSource4 = string.Empty; 
    private string _imageSource5 = string.Empty; 
    private string _selectedImageSource = string.Empty;
    private Brush _imageBorder1Background = new SolidColorBrush(Colors.Transparent);
    private Brush _imageBorder2Background = new SolidColorBrush(Colors.Transparent);
    private Brush _imageBorder3Background = new SolidColorBrush(Colors.Transparent);
    private Brush _imageBorder4Background = new SolidColorBrush(Colors.Transparent);
    private Brush _imageBorder5Background = new SolidColorBrush(Colors.Transparent);
    private Brush _imageBorder1BorderBrush = new SolidColorBrush(Colors.Black);
    private Brush _imageBorder2BorderBrush = new SolidColorBrush(Colors.Black);
    private Brush _imageBorder3BorderBrush = new SolidColorBrush(Colors.Black);
    private Brush _imageBorder4BorderBrush = new SolidColorBrush(Colors.Black);
    private Brush _imageBorder5BorderBrush = new SolidColorBrush(Colors.Black);


    /// <summary>
    /// Lệnh xử lý sự kiện khi chuột di chuyển vào một phần tử (Border).
    /// </summary>
    public ICommand PointerEnteredCommand
    {
        get;
    }

    /// <summary>
    /// Lệnh xử lý sự kiện khi chuột di chuyển ra khỏi một phần tử (Border).
    /// </summary>
    public ICommand PointerExitedCommand
    {
        get;
    }

    /// <summary>
    /// Lệnh xử lý sự kiện khi chuột được nhấn lên một phần tử (Border).
    /// </summary>
    public ICommand PointerPressedCommand
    {
        get;
    }

    /// <summary>
    /// Lệnh xử lý sự kiện khi chuột được thả ra khỏi một phần tử (Border).
    /// </summary>
    public ICommand PointerReleasedCommand
    {
        get;
    }

    /// <summary>
    /// Lệnh điều hướng về trang trước (Guide1ViewModel).
    /// </summary>
    public ICommand NavigateBackCommand
    {
        get;
    }

    /// <summary>
    /// Lệnh điều hướng tới trang tiếp theo (Guide3ViewModel).
    /// </summary>
    public ICommand NavigateNextCommand
    {
        get;
    }

    private readonly INavigationService _navigationService;
    private readonly IWindowService _windowService;
    private readonly IDialogService _dialogService;
    public string ImageSource1
    {
        get => _imageSource1;
        set => SetProperty(ref _imageSource1, value);
    }

    public string ImageSource2
    {
        get => _imageSource2;
        set => SetProperty(ref _imageSource2, value);
    }

    public string ImageSource3
    {
        get => _imageSource3;
        set => SetProperty(ref _imageSource3, value);
    }

    public string ImageSource4
    {
        get => _imageSource4;
        set => SetProperty(ref _imageSource4, value);
    }

    public string ImageSource5
    {
        get => _imageSource5;
        set => SetProperty(ref _imageSource5, value);
    }

    public Brush ImageBorder1Background
    {
        get => _imageBorder1Background;
        set => SetProperty(ref _imageBorder1Background, value);
    }

    public Brush ImageBorder2Background
    {
        get => _imageBorder2Background;
        set => SetProperty(ref _imageBorder2Background, value);
    }

    public Brush ImageBorder3Background
    {
        get => _imageBorder3Background;
        set => SetProperty(ref _imageBorder3Background, value);
    }

    public Brush ImageBorder4Background
    {
        get => _imageBorder4Background;
        set => SetProperty(ref _imageBorder4Background, value);
    }

    public Brush ImageBorder5Background
    {
        get => _imageBorder5Background;
        set => SetProperty(ref _imageBorder5Background, value);
    }

    public Brush ImageBorder1BorderBrush
    {
        get => _imageBorder1BorderBrush;
        set => SetProperty(ref _imageBorder1BorderBrush, value);
    }

    public Brush ImageBorder2BorderBrush
    {
        get => _imageBorder2BorderBrush;
        set => SetProperty(ref _imageBorder2BorderBrush, value);
    }

    public Brush ImageBorder3BorderBrush
    {
        get => _imageBorder2BorderBrush;
        set => SetProperty(ref _imageBorder3BorderBrush, value);
    }

    public Brush ImageBorder4BorderBrush
    {
        get => _imageBorder2BorderBrush;
        set => SetProperty(ref _imageBorder4BorderBrush, value);
    }

    public Brush ImageBorder5BorderBrush
    {
        get => _imageBorder2BorderBrush;
        set => SetProperty(ref _imageBorder5BorderBrush, value);
    }

    public string SelectedImageSource
    {
        get => _selectedImageSource;
        set => SetProperty(ref _selectedImageSource, value);
    }
    private bool _isValid = false;
    public bool IsValid
    {
        get => _isValid;
        set => SetProperty(ref _isValid, value);
    }


    /// <brief> Khởi tạo ViewModel với các dịch vụ điều hướng, cửa sổ và hộp thoại. </brief>
    /// <param name="navigationService"> Dịch vụ điều hướng. </param>
    /// <param name="windowService"> Dịch vụ cửa sổ. </param>
    /// <param name="dialogService"> Dịch vụ hộp thoại. </param>
    /// <details> Hàm khởi tạo này thiết lập các lệnh (commands) và cấu hình cửa sổ cho ứng dụng. </details>
    public Guide2ViewModel(INavigationService navigationService, IWindowService windowService, IDialogService dialogService)
    {
        _navigationService = navigationService;
        _windowService = windowService;
        _dialogService = dialogService;
        PointerEnteredCommand = new RelayCommand<Border?>(OnPointerEntered);
        PointerExitedCommand = new RelayCommand<Border?>(OnPointerExited);
        PointerPressedCommand = new RelayCommand<Border?>(OnPointerPressed);
        PointerReleasedCommand = new RelayCommand<Border?>(OnPointerReleased);
        NavigateBackCommand = new RelayCommand(NavigateBack);
        NavigateNextCommand = new RelayCommand(NavigateNext);
        _windowService.SetWindowSize(1200, 800);
        _windowService.SetIsMaximizable(false);
        _windowService.SetIsResizable(false);
    }

    /// <brief> Xử lý sự kiện khi con trỏ di chuyển vào Border. </brief>
    /// <param name="border"> Border mà con trỏ đang di chuyển vào. </param>
    /// <details> Thay đổi màu sắc viền và nền của Border khi con trỏ di chuyển vào. </details>
    private void OnPointerEntered(Border? border)
    {
        if (border != null && border != _selectedBorder)
        {
            border.BorderBrush = new SolidColorBrush(Colors.Blue);
            border.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 81, 93, 239)); // #515DEF
        }
        IsValid = true;
    }

    /// <brief> Xử lý sự kiện khi con trỏ di chuyển ra khỏi Border. </brief>
    /// <param name="border"> Border mà con trỏ đang di chuyển ra khỏi. </param>
    /// <details> Khôi phục màu sắc viền và nền của Border khi con trỏ di chuyển ra khỏi. </details>
    private void OnPointerExited(Border? border)
    {
        if (border != null && border != _selectedBorder)
        {
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.Background = new SolidColorBrush(Colors.Transparent);
        }
    }

    /// <brief> Xử lý sự kiện khi con trỏ được nhấn vào Border. </brief>
    /// <param name="border"> Border mà con trỏ đang nhấn vào. </param>
    /// <details> Thay đổi màu sắc viền và nền của Border khi nhấn và cập nhật thông tin ảnh. </details>
    private void OnPointerPressed(Border? border)
    {
        if (border != null)
        {
            if (_selectedBorder != null)
            {
                _selectedBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                _selectedBorder.Background = new SolidColorBrush(Colors.Transparent);
            }

            _selectedBorder = border;
            border.BorderBrush = new SolidColorBrush(Colors.DarkBlue);
            border.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 61, 73, 189));

            if (border.Tag is string imageSource)
            {
                SelectedImageSource = imageSource;
                _customerInfo.BodyFat = GetBodyFatFromImgSource(imageSource);
            }
        }
    }

    /// <brief> Lấy tỷ lệ mỡ cơ thể từ tên tệp hình ảnh. </brief>
    /// <param name="imgsrc"> Tên tệp hình ảnh (đường dẫn). </param>
    /// <returns> Tỷ lệ mỡ cơ thể tương ứng với hình ảnh. </returns>
    /// <details> Phân tích tên tệp để xác định tỷ lệ mỡ cơ thể từ tên hình ảnh. </details>
    private double GetBodyFatFromImgSource(string imgsrc)
    {
        int lastSlashIndex = imgsrc.LastIndexOf('/');
        string fileNameWithExtension = imgsrc.Substring(lastSlashIndex + 1);
        int lastDotIndex = fileNameWithExtension.LastIndexOf('.');
        string fileNameWithoutExtension = fileNameWithExtension.Substring(0, lastDotIndex);

        switch (fileNameWithoutExtension)
        {
            case "m_1":
                return 31.5;
            case "m_2":
                return 23.5;
            case "m_3":
                return 16.5;
            case "m_4":
                return 12.0;
            case "m_5":
                return 7.5;
            case "f_1":
                return 31.5;
            case "f_2":
                return 27.0;
            case "f_3":
                return 20.5;
            case "f_4":
                return 16.5;
            case "f_5":
                return 14.5;
            default:
                return 0.0;
        }
    }

    /// <brief> Lấy nguồn hình ảnh từ tỷ lệ mỡ cơ thể. </brief>
    /// <param name="bodyFat"> Tỷ lệ mỡ cơ thể. </param>
    /// <param name="isMale"> True nếu giới tính là nam, false nếu là nữ. </param>
    /// <returns> Đường dẫn hình ảnh tương ứng với tỷ lệ mỡ cơ thể. </returns>
    /// <details> Chuyển đổi tỷ lệ mỡ cơ thể và giới tính thành đường dẫn đến hình ảnh tương ứng. </details>
    private string GetImgSourceFromBodyFat(double bodyFat, bool isMale)
    {
        string prefix = isMale ? "m_" : "f_";
        string fileName = bodyFat switch
        {
            31.5 => $"{prefix}1.png",
            23.5 => $"{prefix}2.png",
            16.5 when isMale => $"{prefix}3.png",
            16.5 when !isMale => $"{prefix}4.png",
            12.0 => $"{prefix}4.png",
            7.5 => $"{prefix}5.png",
            27.0 => $"{prefix}2.png",
            20.5 => $"{prefix}3.png",
            14.5 => $"{prefix}5.png",
            _ => throw new ArgumentException("Invalid body fat percentage")
        };

        return $"ms-appx:///Assets/BodyFat/{fileName}";
    }

    /// <brief> Làm nổi bật hình ảnh đã chọn. </brief>
    /// <details> Thay đổi màu sắc viền và nền của Border tương ứng với hình ảnh đã chọn. </details>
    private void HighlightSelectedImage()
    {
        if (SelectedImageSource == ImageSource1)
        {
            ImageBorder1BorderBrush = new SolidColorBrush(Colors.Blue);
            ImageBorder1Background = new SolidColorBrush(ColorHelper.FromArgb(255, 81, 93, 239));
        }
        else if (SelectedImageSource == ImageSource2)
        {
            ImageBorder2BorderBrush = new SolidColorBrush(Colors.Blue);
            ImageBorder2Background = new SolidColorBrush(ColorHelper.FromArgb(255, 81, 93, 239));
        }
        else if (SelectedImageSource == ImageSource3)
        {
            ImageBorder3BorderBrush = new SolidColorBrush(Colors.Blue);
            ImageBorder3Background = new SolidColorBrush(ColorHelper.FromArgb(255, 81, 93, 239));
        }
        else if (SelectedImageSource == ImageSource4)
        {
            ImageBorder4BorderBrush = new SolidColorBrush(Colors.Blue);
            ImageBorder4Background = new SolidColorBrush(ColorHelper.FromArgb(255, 81, 93, 239));
        }
        else if (SelectedImageSource == ImageSource5)
        {
            ImageBorder5BorderBrush = new SolidColorBrush(Colors.Blue);
            ImageBorder5Background = new SolidColorBrush(ColorHelper.FromArgb(255, 81, 93, 239));
        }
        IsValid = true;
    }

    /// <brief> Xử lý sự kiện khi con trỏ được thả ra khỏi Border. </brief>
    /// <param name="border"> Border mà con trỏ vừa thả ra. </param>
    /// <details> Thay đổi màu sắc viền và nền của Border khi con trỏ được thả ra. </details>
    private void OnPointerReleased(Border? border)
    {
        if (border != null && border == _selectedBorder)
        {
            border.BorderBrush = new SolidColorBrush(Colors.Blue);
            border.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 81, 93, 239));
        }
    }

    /// <summary>
    /// Xử lý khi trang được điều hướng tới và cập nhật thông tin khách hàng.
    /// </summary>
    /// <param name="parameter"> Tham số được truyền vào, chứa thông tin khách hàng (CustomerInfo). </param>
    /// <details> Cập nhật các hình ảnh liên quan đến tỷ lệ mỡ cơ thể tùy theo giới tính của khách hàng và 
    /// nếu có tỷ lệ mỡ cơ thể, hình ảnh đã chọn sẽ được làm nổi bật. </details>
    public void OnNavigatedTo(object parameter)
    {
        if (parameter is CustomerInfo customerInfo)
        {
            _customerInfo = customerInfo;
            if (_customerInfo.sex == 1)
            {
                ImageSource1 = "ms-appx:///Assets/BodyFat/m_5.png";
                ImageSource2 = "ms-appx:///Assets/BodyFat/m_4.png";
                ImageSource3 = "ms-appx:///Assets/BodyFat/m_3.png";
                ImageSource4 = "ms-appx:///Assets/BodyFat/m_2.png";
                ImageSource5 = "ms-appx:///Assets/BodyFat/m_1.png";
            }
            else
            {
                ImageSource1 = "ms-appx:///Assets/BodyFat/f_5.png";
                ImageSource2 = "ms-appx:///Assets/BodyFat/f_4.png";
                ImageSource3 = "ms-appx:///Assets/BodyFat/f_3.png";
                ImageSource4 = "ms-appx:///Assets/BodyFat/f_2.png";
                ImageSource5 = "ms-appx:///Assets/BodyFat/f_1.png";
            }
            if (_customerInfo.BodyFat != 0)
            {
                SelectedImageSource = GetImgSourceFromBodyFat(_customerInfo.BodyFat, _customerInfo.sex == 1);
                HighlightSelectedImage();
            }
        }
    }

    /// <summary>
    /// Xử lý khi trang được điều hướng khỏi, không thực hiện hành động gì.
    /// </summary>
    public void OnNavigatedFrom()
    {
    }

    /// <summary>
    /// Chuyển đến trang trước đó (Guide1ViewModel) với thông tin khách hàng.
    /// </summary>
    private void NavigateBack()
    {
        var pageKey = typeof(Guide1ViewModel).FullName;
        if (pageKey != null)
        {
            _navigationService.NavigateTo(pageKey, _customerInfo);
        }
    }

    /// <summary>
    /// Chuyển đến trang tiếp theo (Guide3ViewModel) nếu thông tin hợp lệ.
    /// </summary>
    /// <remarks> Nếu thông tin không hợp lệ, hiển thị hộp thoại lỗi yêu cầu chọn thông tin BodyFat. </remarks>
    private async void NavigateNext()
    {
        if (IsValid == true)
        {
            var pageKey = typeof(Guide3ViewModel).FullName;
            if (pageKey != null)
            {
                _navigationService.NavigateTo(pageKey, _customerInfo);
            }
        }
        else
        {
            await _dialogService.ShowErrorDialogAsync("Vui lòng chọn thông tin BodyFat");
        }
    }
}
