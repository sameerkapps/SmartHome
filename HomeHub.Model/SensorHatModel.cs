/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * License: MIT License
***********************************************************************************************/
using System;

namespace HomeHub.Model
{
    /// <summary>
    /// Model to transfer sense hat related data between the client and the WebAPI
    /// </summary>
    public class SensorHatModel
    {
        /// <summary>
        /// Id of the device
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Temperature
        /// </summary>
        public int Temperature { get; set; }

        /// <summary>
        /// Humidity
        /// </summary>
        public int Humidity { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
