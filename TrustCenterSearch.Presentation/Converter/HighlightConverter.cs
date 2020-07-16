using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace TrustCenterSearch.Presentation.Converter
{
    internal class HighlightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is String filterInput))
                return false;
            if (!(parameter is TextBox textBox))
                return false;

            if (textBox.Text != String.Empty && filterInput.ToLower().Contains(textBox.Text.ToLower()))
            {
                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
