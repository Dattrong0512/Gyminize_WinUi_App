using CommunityToolkit.Mvvm.ComponentModel;

using Gyminize.Contracts.Services;
using Gyminize.ViewModels;
using Gyminize.Views;

using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    // Khởi tạo PageService và cấu hình các trang.
    public PageService()
    {
        Configure<HomeViewModel, HomePage>();
        
        Configure<NutritionsViewModel, NutritionsPage>();
        Configure<DiaryViewModel, DiaryPage>();
        Configure<ShopViewModel, ShopPage>();
        //Configure<ShopDetailViewModel, ShopDetailPage>();
        Configure<SigninViewmodel, SigninPage>(); // Thêm dòng này
        Configure<Guide1ViewModel, GuidePage1>();
        Configure<Guide2ViewModel, GuidePage2>();
        Configure<Guide3ViewModel, GuidePage3>();
        Configure<SignupViewModel, SignupPage>();
        Configure<ShellViewModel, ShellPage>();
        Configure<PlanSelectionViewModel, PlanSelectionPage>();
        Configure<PlanViewModel, PlanPage>();
        Configure<ChatBoxViewModel, ChatBoxPage>();
    }

    // Lấy loại trang dựa trên khóa trang.
    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    // Cấu hình trang với ViewModel và View cụ thể.
    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
