using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

namespace rwpf.TypeConverters
{
    /// <summary>
    /// Conversion between string and IPAddress for XAML
    /// </summary>
    public class IPAddressConverter : TypeConverter
    {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(IPAddress))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    return IPAddress.Parse(value as string);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Cannot convert '{0}' ({1}) because {2}", value, value.GetType(), ex.Message), ex);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            if(value is IPAddress)
            {
                return ((IPAddress)value).ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
