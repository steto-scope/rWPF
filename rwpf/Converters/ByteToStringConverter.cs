using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace rwpf.Converters
{
    /// <summary>
    /// Converts a single Byte into a String and vice versa. Extends the default behaviour in the way that an empty string is considered (byte)0
    /// </summary>
    internal class ByteToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is byte))
                return Binding.DoNothing;

            byte b = (byte)value;
            return ((int)b).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value!=null)
            {
                byte b;
                if (byte.TryParse(value.ToString(), out b))
                    return b;
                else
                    return Binding.DoNothing;
            }
            return (byte)0;
        }
    }
}
