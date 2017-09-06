////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;

namespace SmartHomePiApp.Device
{
    /// <summary>
    /// Simulator for the hardware
    /// </summary>
    public class SimHardware : IHardware
    {
        /// <summary>
        /// Update sensors. Not required in the sim
        /// </summary>
        public void UpdateSensors()
        {
            // do nothing
        }

        /// <summary>
        /// no action required
        /// </summary>
        /// <param name="lightId"></param>
        /// <param name="on"></param>
        public void SwitchLight(string lightId, bool on)
        {

        }

        /// <summary>
        /// Generates random temperature and returns it
        /// </summary>
        public int Temperature
        {
            get
            {
                var rand = new Random();

                return rand.Next(55, 85);
            }
        }

        /// <summary>
        /// Generates random humidity and returns it
        /// </summary>
        public int Humidity
        {
            get
            {
                var rand = new Random();

                return rand.Next(20, 50);
            }
        }

        /// <summary>
        /// Id of the device
        /// </summary>
        public string DeviceId
        {
            get;
        } = "SimHardware";

    }
}
