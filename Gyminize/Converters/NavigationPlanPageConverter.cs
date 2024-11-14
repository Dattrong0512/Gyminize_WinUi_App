using System;
using Microsoft.UI.Xaml.Data;
using Windows.UI.Xaml.Data;

namespace Gyminize.Converters;

public class NavigationPlanPageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var condition = (bool)value;
        return condition ? "Gyminize.ViewModels.PlanSelectionViewModel" : "Gyminize.ViewModels.PlanViewModel";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
