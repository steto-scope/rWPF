using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace rwpf.Converters
{
    public class BoolToCursorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to a Cursor. By default Cursors.Hand will be used.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Cursor object to be used</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if(parameter!=null && parameter is Cursor)
                    return (Cursor)parameter;    

                return Cursors.Hand;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException("BoolToCursorConverter can only be used OneWay.");
        }
    }
}
