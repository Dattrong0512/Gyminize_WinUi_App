using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;

namespace Gyminize.Converters;

public class PriceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal price)
        {
            return price.ToString("#,##0", new CultureInfo("vi-VN"));
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}