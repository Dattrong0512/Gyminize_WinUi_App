﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;

namespace Gyminize.Converters;

public class DayBorderBackGroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (bool)value ? new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x3D, 0x52, 0xA0)) : new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xED, 0xE8, 0xF5));
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}