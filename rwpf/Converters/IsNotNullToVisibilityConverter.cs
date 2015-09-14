using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace rwpf.Converters
{
    class IsNotNullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;
            if(parameter!=null && parameter is Visibility)
            {
                v = (Visibility)parameter;
            }
            return value != null ? Visibility.Visible : v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException("IsNotNullToVisibilityConverter can only be used OneWay.");
        }
    }
}
