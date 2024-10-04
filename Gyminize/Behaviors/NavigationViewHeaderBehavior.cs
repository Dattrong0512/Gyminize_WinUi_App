using Gyminize.Contracts.Services;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Xaml.Interactivity;

namespace Gyminize.Behaviors;

// Lớp này quản lý tiêu đề của NavigationView dựa trên các thuộc tính của trang hiện tại
public class NavigationViewHeaderBehavior : Behavior<NavigationView>
{
    // Biến tĩnh để giữ hành vi hiện tại
    private static NavigationViewHeaderBehavior? _current;

    // Giữ trang hiện tại đang được hiển thị
    private Page? _currentPage;

    // Thuộc tính để thiết lập mẫu tiêu đề mặc định
    public DataTemplate? DefaultHeaderTemplate
    {
        get; set;
    }

    // Thuộc tính để thiết lập nội dung tiêu đề mặc định
    public object DefaultHeader
    {
        get => GetValue(DefaultHeaderProperty);
        set => SetValue(DefaultHeaderProperty, value);
    }

    // Thuộc tính phụ thuộc cho DefaultHeader
    public static readonly DependencyProperty DefaultHeaderProperty =
        DependencyProperty.Register("DefaultHeader", typeof(object), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => _current!.UpdateHeader()));

    // Phương thức để lấy HeaderMode của một trang
    public static NavigationViewHeaderMode GetHeaderMode(Page item) => (NavigationViewHeaderMode)item.GetValue(HeaderModeProperty);

    // Phương thức để thiết lập HeaderMode của một trang
    public static void SetHeaderMode(Page item, NavigationViewHeaderMode value) => item.SetValue(HeaderModeProperty, value);

    // Thuộc tính phụ thuộc cho HeaderMode
    public static readonly DependencyProperty HeaderModeProperty =
        DependencyProperty.RegisterAttached("HeaderMode", typeof(NavigationViewHeaderMode), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(NavigationViewHeaderMode.Always, (d, e) => _current!.UpdateHeader()));

    // Phương thức để lấy HeaderContext của một trang
    public static object GetHeaderContext(Page item) => item.GetValue(HeaderContextProperty);

    // Phương thức để thiết lập HeaderContext của một trang
    public static void SetHeaderContext(Page item, object value) => item.SetValue(HeaderContextProperty, value);

    // Thuộc tính phụ thuộc cho HeaderContext
    public static readonly DependencyProperty HeaderContextProperty =
        DependencyProperty.RegisterAttached("HeaderContext", typeof(object), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => _current!.UpdateHeader()));

    // Phương thức để lấy HeaderTemplate của một trang
    public static DataTemplate GetHeaderTemplate(Page item) => (DataTemplate)item.GetValue(HeaderTemplateProperty);

    // Phương thức để thiết lập HeaderTemplate của một trang
    public static void SetHeaderTemplate(Page item, DataTemplate value) => item.SetValue(HeaderTemplateProperty, value);

    // Thuộc tính phụ thuộc cho HeaderTemplate
    public static readonly DependencyProperty HeaderTemplateProperty =
        DependencyProperty.RegisterAttached("HeaderTemplate", typeof(DataTemplate), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => _current!.UpdateHeaderTemplate()));

    // Được gọi khi hành vi được gắn vào NavigationView
    protected override void OnAttached()
    {
        base.OnAttached();

        // Lấy dịch vụ điều hướng và đăng ký sự kiện Navigated
        var navigationService = App.GetService<INavigationService>();
        navigationService.Navigated += OnNavigated;

        // Thiết lập instance hiện tại
        _current = this;
    }

    // Được gọi khi hành vi được tách khỏi NavigationView
    protected override void OnDetaching()
    {
        base.OnDetaching();

        // Lấy dịch vụ điều hướng và hủy đăng ký sự kiện Navigated
        var navigationService = App.GetService<INavigationService>();
        navigationService.Navigated -= OnNavigated;
    }

    // Trình xử lý sự kiện cho các sự kiện điều hướng
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame && frame.Content is Page page)
        {
            // Thiết lập trang hiện tại
            _currentPage = page;

            // Cập nhật tiêu đề và mẫu tiêu đề
            UpdateHeader();
            UpdateHeaderTemplate();
        }
    }

    // Cập nhật tiêu đề dựa trên HeaderMode và HeaderContext của trang hiện tại
    private void UpdateHeader()
    {
        if (_currentPage != null)
        {
            var headerMode = GetHeaderMode(_currentPage);
            if (headerMode == NavigationViewHeaderMode.Never)
            {
                // Ẩn tiêu đề nếu chế độ là Never
                AssociatedObject.Header = null;
                AssociatedObject.AlwaysShowHeader = false;
            }
            else
            {
                // Thiết lập tiêu đề từ HeaderContext của trang hoặc sử dụng tiêu đề mặc định
                var headerFromPage = GetHeaderContext(_currentPage);
                if (headerFromPage != null)
                {
                    AssociatedObject.Header = headerFromPage;
                }
                else
                {
                    AssociatedObject.Header = DefaultHeader;
                }

                // Hiển thị hoặc ẩn tiêu đề dựa trên chế độ
                if (headerMode == NavigationViewHeaderMode.Always)
                {
                    AssociatedObject.AlwaysShowHeader = true;
                }
                else
                {
                    AssociatedObject.AlwaysShowHeader = false;
                }
            }
        }
    }

    // Cập nhật mẫu tiêu đề dựa trên HeaderTemplate của trang hiện tại
    private void UpdateHeaderTemplate()
    {
        if (_currentPage != null)
        {
            var headerTemplate = GetHeaderTemplate(_currentPage);
            AssociatedObject.HeaderTemplate = headerTemplate ?? DefaultHeaderTemplate;
        }
    }
}
