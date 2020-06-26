﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TrustCenterSearch.Presentation.Converter
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (String.IsNullOrEmpty((string) value))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
