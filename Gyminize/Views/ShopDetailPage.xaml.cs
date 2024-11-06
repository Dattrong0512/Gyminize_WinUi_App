using CommunityToolkit.WinUI.UI.Animations;

using Gyminize.Contracts.Services;
using Gyminize.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Gyminize.Views;

public sealed partial class ShopDetailPage : Page
{
    //public ShopDetailViewModel ViewModel
    //{
    //    get;
    //}

    //public ShopDetailPage()
    //{
    //    ViewModel = App.GetService<ShopDetailViewModel>();
    //    InitializeComponent();
    //}

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        //this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        //if (e.NavigationMode == NavigationMode.Back)
        //{
        //    var navigationService = App.GetService<INavigationService>();

        //    if (ViewModel.Item != null)
        //    {
        //        navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
        //    }
        //}
    }
}
