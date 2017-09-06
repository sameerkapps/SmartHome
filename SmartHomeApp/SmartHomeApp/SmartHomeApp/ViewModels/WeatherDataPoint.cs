////////////////////////////////////////////////////////////
// Copyright 2017 Sameer Khandekar                        //
// License: MIT License                                   //
////////////////////////////////////////////////////////////
using System;

namespace HomeMonitor.ViewModels
{
    public class WeatherDataPoint
    {
        /// <summary>
        /// Time of the datapoint
        /// </summary>
        /// <value>The hour.</value>
        public DateTime Time
        {
            get;
            set;
        }

        /// <summary>
        /// Temperature
        /// </summary>
        public int Temperature
        {
            get;
            set;
        }
    }
}
