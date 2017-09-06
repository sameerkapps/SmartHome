////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;

using Emmellsoft.IoT.Rpi.SenseHat;

namespace SmartHomePiApp.Device
{
    /// <summary>
    /// Class to interact with the sense hat
    /// </summary>
    public class SensorHatHardwareClient : IHardware
    {
        /// <summary>
        /// Updates the sensors
        /// </summary>
        public void UpdateSensors()
        {
            // make sure that eh hat is activated
            EnsureSenseHatActivated();
            // this will update readings on Humidity and temperature
            _senseHat.Sensors.HumiditySensor.Update();
        }

        /// <summary>
        /// Turns the display to Red for On and blank for off
        /// </summary>
        /// <param name="lightId"></param>
        /// <param name="on"></param>
        public void SwitchLight(string lightId, bool on)
        {
            EnsureSenseHatActivated();
            _senseHat.Display.Fill(on ? Colors.Red : Colors.Black);
            _senseHat.Display.Update();
        }

        /// <summary>
        /// Temperature in fahrenheit. 
        /// </summary>
        public int Temperature
        {
            get
            {
                var cel = (_senseHat.Sensors.Temperature ?? 0.0);
                var far = ((cel * 9) / 5) + 32;
                return (int)far;
            }
        }

        /// <summary>
        /// Humidity as percent
        /// </summary>
        public int Humidity
        {
            get
            {
                return (int)(_senseHat.Sensors.Humidity ?? 0.0);
            }
        }

        /// <summary>
        /// Id of the device
        /// </summary>
        public string DeviceId
        {
            get;
        } = "SensorHat";

        // sense hat
        private ISenseHat _senseHat;

        #region private methods
        /// <summary>
        /// Makes sure tat Sensehat is instantiated
        /// and initiated
        /// </summary>
        private void EnsureSenseHatActivated()
        {
            // if sense hat is not created
            // create it and set the display blank
            if (_senseHat == null)
            {
                ManualResetEvent evt = new ManualResetEvent(false);
                Task.Run(async () =>
                {
                    _senseHat = await SenseHatFactory.GetSenseHat();
                    _senseHat.Display.Fill(Color.FromArgb(0, 0, 0, 0));
                    _senseHat.Display.Update();
                    evt.Set();
                });

                evt.WaitOne();
            }
        }

        #endregion
    }
}
