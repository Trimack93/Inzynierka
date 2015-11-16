using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApplication1.Utilities
{
    public class StringToNewlineStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value as string;

            if ( string.IsNullOrEmpty(stringValue) )
                return value;

            string validNewLine = stringValue.Replace("\\n", "\n");                 // String in XAML identificates "\n" as "\\n" by default, so we need to change it first

            return Regex.Replace(validNewLine, @"[^\S\r\n]{2,}", string.Empty);     // Regex which deletes additional whitespaces added do string by VS after each new line of code
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
