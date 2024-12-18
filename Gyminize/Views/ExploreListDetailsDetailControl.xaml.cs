using Gyminize.Core.Models;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Gyminize.ViewModels;
using Gyminize.Core.Contracts.Services;
using Gyminize.Core.Services;
using Gyminize.Services;
using Gyminize.Contracts.Services;

namespace Gyminize.Views;

public sealed partial class ExploreListDetailsDetailControl : UserControl
{
    public Influencer? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as Influencer;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public ExploreListDetailsViewModel ViewModel
    {
        get => (ExploreListDetailsViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof(ExploreListDetailsViewModel),
            typeof(ExploreListDetailsDetailControl),
            new PropertyMetadata(null));
    

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(Influencer), typeof(ExploreListDetailsDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public ExploreListDetailsDetailControl()
    {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ExploreListDetailsDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
