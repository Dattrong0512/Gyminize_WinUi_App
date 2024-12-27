using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace Gyminize.Converters;
public class NumberToCommaSeparatedStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null)
        {
            return string.Empty; // or any default value
        }

        if (value is int intValue)
        {
            return intValue.ToString("N0", CultureInfo.InvariantCulture);
        }
        else if (value is double doubleValue)
        {
            return doubleValue.ToString("N0", CultureInfo.InvariantCulture);
        }
        else if (value is string strValue && double.TryParse(strValue, out double parsedValue))
        {
            return parsedValue.ToString("N0", CultureInfo.InvariantCulture);
        }
        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string strValue && double.TryParse(strValue, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double result))
        {
            return result;
        }
        return value;
    }
}
