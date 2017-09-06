////////////////////////////////////////////////////////////
// Copyright 2017 Sameer Khandekar                        //
// License: MIT License                                   //
////////////////////////////////////////////////////////////
using System;
using System.Collections.ObjectModel;

using HomeMonitor.Commands;
using MvvmAtom;

using HomeHubClient;
using SmartHomeApp;

namespace HomeMonitor.ViewModels
{
    /// <summary>
    /// View model
    /// </summary>
    public class MainPageViewModel : MvvmAtom.AtomViewModelBase
    {
        /// <summary>
        /// Temp and humidity data
        /// </summary>
        public ObservableCollection<WeatherDataPoint> WeatherData { get; } = new ObservableCollection<WeatherDataPoint>();

        /// <summary>
        /// Command to rerieve data
        /// </summary>
        public AtomCommandBase RetrieveWeatherData
        {
            get;
        }

        private bool _isLEDOn;
        /// <summary>
        /// Turn LED on/off
        /// </summary>
        public bool IsLEDOn
        {
            get
            {
                return _isLEDOn;
            }

            set
            {
                if (_isLEDOn != value)
                {
                    _isLEDOn = value;
                    SwitchLED();
                }
            }
        }

        /// <summary>
        /// Constructor initializes command
        /// </summary>
        public MainPageViewModel()
        {
            RetrieveWeatherData = new GetWeatherDataCommand(this);
        }

        /// <summary>
        /// Asks the server to turn LED on/off
        /// </summary>
        private async void SwitchLED()
        {
            using (var light = new Light(Settings.CloudBaseAddress, Settings.ValidationHeaderKey))
            {
                await light.SwitchLight("123", IsLEDOn);
            }
        }
    }
}
