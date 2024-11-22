// Tệp App.xaml.cs chứa logic khởi động và cấu hình ứng dụng.
using Gyminize.Activation;
using Gyminize.Contracts.Services;
using Gyminize.Core.Contracts.Services;
using Gyminize.Core.Services;
using Gyminize.Helpers;
using Gyminize.Models;
using Gyminize.Services;
using Gyminize.ViewModels;
using Gyminize.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Gyminize;

// Lớp App kế thừa từ Application để đại diện cho ứng dụng.
public partial class App : Application
{
    // Thuộc tính Host cung cấp các dịch vụ như dependency injection, cấu hình, logging, và các dịch vụ khác.
    public IHost Host
    {
        get;
    }

    // Phương thức tĩnh để lấy dịch vụ đã đăng ký trong Host.
    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    // Thuộc tính MainWindow đại diện cho cửa sổ chính của ứng dụng.
    public static WindowEx MainWindow { get; } = new MainWindow();

    // Thuộc tính AppTitlebar để lưu trữ tiêu đề của ứng dụng.
    public static UIElement? AppTitlebar
    {
        get; set;
    }

    // Constructor của lớp App.
    public App()
    {
        InitializeComponent();

        // Khởi tạo Host với các dịch vụ cần thiết.
        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Đăng ký Activation Handlers.
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Đăng ký các dịch vụ khác.
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<IWindowService, WindowService>();

            services.AddSingleton<IDialogService, DialogService>();

            // Core Services
            //services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IFileService, FileService>();


            // Đăng ký Views và ViewModels.
            //services.AddTransient<ShopDetailViewModel>();
            //services.AddTransient<ShopViewModel>();
            //services.AddTransient<ShopDetailPage>();

            //services.AddTransient<ShopPage>();
            services.AddTransient<DiaryViewModel>();
            services.AddTransient<DiaryPage>();
            services.AddTransient<NutritionsViewModel>();
            services.AddTransient<NutritionsPage>();
            services.AddTransient<PlanViewModel>();
            services.AddTransient<PlanPage>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<HomePage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<SigninPage>();
            services.AddTransient<Guide1ViewModel>();
            services.AddTransient<GuidePage1>();
            services.AddTransient<Guide2ViewModel>();
            services.AddTransient<GuidePage2>();
            services.AddTransient<SigninViewmodel>();
            services.AddTransient<Guide3ViewModel>();
            services.AddTransient<GuidePage3>();
            services.AddTransient<SignupPage>();
            services.AddTransient<SignupViewModel>();
            services.AddTransient<PlanSelectionPage>();
            services.AddTransient<PlanSelectionViewModel>();

            services.AddSingleton<Customer>();
            // Đăng ký cấu hình.
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        // Đăng ký sự kiện xử lý ngoại lệ không được xử lý.
        UnhandledException += App_UnhandledException;
    }

    // Phương thức xử lý ngoại lệ không được xử lý.
    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log và xử lý ngoại lệ theo cách phù hợp.
    }

    // Phương thức được gọi khi ứng dụng được khởi chạy.
    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

       

        // Kích hoạt dịch vụ kích hoạt
        await App.GetService<IActivationService>().ActivateAsync(args);
    }
    
}
