using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CrossStitch.App.Converters
{
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(value as string);
                    return new SolidColorBrush(color);
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}