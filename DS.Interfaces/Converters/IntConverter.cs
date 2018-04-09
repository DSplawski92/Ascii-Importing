using System;
using System.Windows.Data;

namespace DS.Interfaces
{
    public class IntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();      
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int convertedValue;
            string stringValue = value as string;
            return stringValue == string.Empty ? 
                0 : int.TryParse(stringValue, out convertedValue) ?
                convertedValue : 0;
        }
    }
}
