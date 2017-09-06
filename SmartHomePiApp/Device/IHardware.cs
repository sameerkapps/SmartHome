////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;

namespace SmartHomePiApp.Device
{
    /// <summary>
    /// Interface for the hardware
    /// </summary>
    public interface IHardware
    {
        /// <summary>
        /// ID of the device
        /// </summary>
        string DeviceId { get; }

        /// <summary>
        /// Temperature
        /// </summary>
        int Temperature { get; }

        /// <summary>
        /// Humidity
        /// </summary>
        int Humidity { get; }

        /// <summary>
        /// Asks the SenseHat to update the sensors
        /// </summary>
        void UpdateSensors();

        /// <summary>
        /// Turn the light on/off
        /// </summary>
        /// <param name="lightId"></param>
        /// <param name="on"></param>
        void SwitchLight(string lightId, bool on);
    }
}
