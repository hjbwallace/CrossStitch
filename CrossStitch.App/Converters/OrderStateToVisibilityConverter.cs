using CrossStitch.Core.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CrossStitch.App.Converters
{
    public class OrderStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is OrderState orderStateCurrent))
                return Visibility.Collapsed;

            if (!(parameter is OrderState orderStateRequired))
                return Visibility.Collapsed;

            return orderStateCurrent == orderStateRequired ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}