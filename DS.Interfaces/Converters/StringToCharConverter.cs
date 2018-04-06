using System;
using System.Linq;
using System.Windows.Data;

namespace DS.Interfaces
{
    public class StringToCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();      
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString() != string.Empty ? value.ToString().First() : '\0';
        }
    }
}
