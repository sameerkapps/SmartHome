// ////////////////////////////////////////////////////////////
// // Copyright 2017 Sameer Khandekar                        //
// // License: MIT License                                   //
// ////////////////////////////////////////////////////////////
using System;

using MvvmAtom;

using HomeHubClient;
using HomeMonitor.ViewModels;
using SmartHomeApp;

namespace HomeMonitor.Commands
{
    /// <summary>
    /// Gets the weather data
    /// </summary>
    public class GetWeatherDataCommand : HomeMonitorCommandBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewModel"></param>
        public GetWeatherDataCommand(AtomViewModelBase viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// Can always run the command
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Gets the data from the cloud and updates the view model
        /// </summary>
        /// <param name="parameter"></param>
        public async override void Execute(object parameter)
        {
            using (SensorHat sensorHat = new SensorHat(Settings.CloudBaseAddress, "NotUsed"))
            {
                var weatherData = await sensorHat.GetData(DeviceId, 48);
                MainViewModel.WeatherData.Clear();
                foreach (var dataPoint in weatherData)
                {
                    var weatherDataPt = new WeatherDataPoint();
                    weatherDataPt.Temperature = dataPoint.Temperature;
                    weatherDataPt.Time = dataPoint.TimeStamp;
                    MainViewModel.WeatherData.Add(weatherDataPt);
                }
            }
        }

        // private const string DeviceId = "Seattle_SimHardware";
        private const string DeviceId = "Seattle_SensorHat";
    }
}
