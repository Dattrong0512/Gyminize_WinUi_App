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
        MyCalendarView.MinDate = new DateTimeOffset(DateTime.Now.AddDays(-21));
        MyCalendarView.MaxDate = new DateTimeOffset(DateTime.Now.AddDays(21));
        this.DataContext = ViewModel;
    }

    private void MyCalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
    {
        var daySelected = args.AddedDates.Count > 0 ? args.AddedDates[0].DateTime.ToUniversalTime() : DateTime.UtcNow;
        daySelected = daySelected.ToLocalTime();
        ViewModel.SelectedDatesChangedCommand.Execute(daySelected);
    }

    private void MyCalendarView_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
    {
        ViewModel.DecorateDayItemCommand.Execute(args.Item); // Call decoration logic
    }
}
