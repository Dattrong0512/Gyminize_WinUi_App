using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;

namespace Gyminize.Converters;

public class StatusToPaymentMethodConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,string language)
    {
        if (value is string status)
        {
            return status switch
            {
                "Completed" => "VNPAY",
                "Completed-COD" => "Ship COD",
                _ => "Unknown"
            };
        }
        return "Unknown";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
