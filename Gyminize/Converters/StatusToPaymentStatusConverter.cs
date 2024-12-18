using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;

namespace Gyminize.Converters;

public class StatusToPaymentStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string status)
        {
            return status switch
            {
                "Completed" => "Đã thanh toán",
                "Completed-COD" => "Chưa thanh toán",
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