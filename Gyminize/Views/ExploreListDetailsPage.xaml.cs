using CommunityToolkit.WinUI.UI.Controls;

using Gyminize.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Views;

public sealed partial class ExploreListDetailsPage : Page
{
    public ExploreListDetailsViewModel ViewModel
    {
        get;
    }



    public ExploreListDetailsPage()
    {
        ViewModel = App.GetService<ExploreListDetailsViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
