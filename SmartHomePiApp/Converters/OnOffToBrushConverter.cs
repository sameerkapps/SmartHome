////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;

using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace SmartHomePiApp.Converters
{
    /// <summary>
    /// Converts On/off state to the brush
    /// </summary>
    public class OnOffToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts on to red and off to light gray
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isOn = (bool)value;

            return isOn ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.LightGray);
        }

        /// <summary>
        /// Not applicable
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
