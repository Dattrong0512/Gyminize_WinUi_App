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
        MyCalendarView.MinDate = new DateTimeOffset(new DateTime(2023, 1, 1));
        MyCalendarView.MaxDate = new DateTimeOffset(new DateTime(2025, 12, 31));
    }
}
