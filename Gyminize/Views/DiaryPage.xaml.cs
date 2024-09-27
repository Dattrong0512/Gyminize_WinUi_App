using Gyminize.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Gyminize.Views;

public sealed partial class DiaryPage : Page
{
    public DiaryViewModel ViewModel
    {
        get;
    }

    public DiaryPage()
    {
        ViewModel = App.GetService<DiaryViewModel>();
        InitializeComponent();
    }
}
