////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using Windows.UI.Xaml.Data;

namespace SmartHomePiApp.Converters
{
    /// <summary>
    /// Timer button text is defined by the state
    /// </summary>
    public class TimerStateToTextConverter : IValueConverter
    {
        /// <summary>
        /// Converts state to start/stop text
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isOn = (bool)value;

            return isOn ? "Stop" : "Start";
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
