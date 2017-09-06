////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;

using MvvmAtom;

using HomeHubClient;

using SmartHomePiApp.Commands;
using SmartHomePiApp.Device;

namespace SmartHomePiApp.ViewModels
{
    /// <summary>
    /// View model for the main page
    /// </summary>
    public class MainPageViewModel : AtomViewModelBase
    {
        /// <summary>
        /// Constructor to set up commands and the Hub
        /// </summary>
        public MainPageViewModel()
        {
            ToggleTimer = new ToggleTimerCommand(this);
            SetupHub();
        }


        private int _temperature;
        /// <summary>
        /// Represents the temperature of the device
        /// </summary>
        public int Temperature
        {
            get
            {
                return _temperature;
            }

            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int _humidity;
        /// <summary>
        /// Represents the humidity of the device
        /// </summary>
        public int Humidity
        {
            get
            {
                return _humidity;
            }

            set
            {
                if (_humidity != value)
                {
                    _humidity = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int _durationInSec = 600;
        /// <summary>
        /// Represents the interval ins sec to retrieve the reading
        /// </summary>
        public int DurationInSec
        {
            get
            {
                return _durationInSec;
            }

            set
            {
                if (_durationInSec != value)
                {
                    _durationInSec = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool _useSimulator = true;
        /// <summary>
        /// Use simulator or real hardware
        /// </summary>
        public bool UseSimulator
        {
            get
            {
                return _useSimulator;
            }

            set
            {
                if (_useSimulator != value)
                {
                    _useSimulator = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool _isLEDOn = true;
        /// <summary>
        /// Is LED on/off
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
                    RaisePropertyChanged();
                }
            }
        }

        private bool _isTimerOn;
        /// <summary>
        /// State of the timer
        /// </summary>
        public bool IsTimerOn
        {
            get
            {
                return _isTimerOn;
            }

            set
            {
                if (_isTimerOn != value)
                {
                    if (IsTimerOn)
                    {
                        TurnTimerOff();
                    }
                    else
                    {
                        TurnTimerOn();
                    }
                    _isTimerOn = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Hardware interface. Value is dependent upon the simulator
        /// </summary>
        private IHardware Hardware
        {
            get
            {
                return UseSimulator ? _simHardware : _realHardware;
            }
        }

        /// <summary>
        /// Configure and turn the timer on
        /// </summary>
        private void TurnTimerOn()
        {
            _dispatchTimer.Interval = TimeSpan.FromSeconds(DurationInSec);
            _dispatchTimer.Tick += _dispatchTimer_Tick;
            _dispatchTimer.Start();

            // show immediate one
            try
            {
                /// fire and forget
                GetDataAndSend().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Timer tick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void _dispatchTimer_Tick(object sender, object e)
        {
            await GetDataAndSend();
        }

        /// <summary>
        /// Gets the data from the hardware and sends to the cloud
        /// </summary>
        /// <returns></returns>
        private async Task GetDataAndSend()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                async () =>

                {
                    Hardware.UpdateSensors();
                    Temperature = Hardware.Temperature - TemperatureOffset;
                    Humidity = Hardware.Humidity;

                    await SendToCloud();
                });
        }

        /// <summary>
        /// sends the data to the cloud
        /// </summary>
        /// <returns></returns>
        private async Task SendToCloud()
        {
            try
            {
                using (var sensorHat = new SensorHat(Settings.CloudBaseAddress, Settings.ValidationHeaderKey))
                {
                    await sensorHat.AddData("Seattle_" + Hardware.DeviceId,
                                      Temperature,
                                      Humidity);
                }
            }
            catch (Exception ex)
            {
                // does not matter, if the data does not reach
                // not critical
                // just trace it
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Turns the timer ticks off
        /// </summary>
        private void TurnTimerOff()
        {
            _dispatchTimer.Stop();
            _dispatchTimer.Tick -= _dispatchTimer_Tick;
        }

        public AtomCommandBase ToggleTimer { get; }

        #region private methods
        /// <summary>
        /// Sets up Signal R hub
        /// </summary>
        private void SetupHub()
        {
            // set the hub connection
            var hubConnection = new HubConnection(Settings.CloudBaseAddress);
            // create the proxy
            var hubProxy = hubConnection.CreateHubProxy("SwitchHub");
            // lisen to FlipSwitch
            hubProxy.On<string, bool>("FlipSwitch", FlipIt);
            // start listening
            hubConnection.Start(new LongPollingTransport());
        }

        /// <summary>
        /// Server asks to flip the switch
        /// </summary>
        /// <param name="lightId"></param>
        /// <param name="on"></param>
        private async void FlipIt(string lightId, bool on)
        {
            // send command to the hardware
            Hardware.SwitchLight(lightId, on);
            // show on the screen
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                IsLEDOn = on;
            });
        }

        #endregion

        #region fields
        // timer
        DispatcherTimer _dispatchTimer = new DispatcherTimer();
        // hardware simulator
        private IHardware _simHardware = new SimHardware();
        // real hardware
        private IHardware _realHardware = new SensorHatHardwareClient();
        #endregion

        #region constants
        // this is done to calibrate 
        private const int TemperatureOffset = 12;

        // Hub connection string
        private const string HubConnectionString = "http://localhost:59312/";
        #endregion
    }
}
