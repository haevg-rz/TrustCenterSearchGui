using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace TrustCenterSearch.Presentation.Converter
{
    class HighlightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color;
            var textBox = parameter as TextBox;
            var textBoxText = textBox.Text.ToLower();
            var text = value.ToString().ToLower();
            if ((textBoxText != String.Empty) && (text.Contains(textBoxText)))
            {
                color = "Red";
            }
            else
            {
                color = "Black";
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
