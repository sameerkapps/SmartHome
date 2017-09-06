////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;

using Microsoft.WindowsAzure.Storage.Table;

namespace SmartHomeRESTAPI.TableEntities
{
    /// <summary>
    /// Entity for the sensor hat
    /// </summary>
    public class SensorHatEntity : TableEntity
    {
        /// <summary>
        /// Default constructor is required
        /// </summary>
        public SensorHatEntity()
        {
        }

        /// <summary>
        /// Entity for a row. can be forward/backward enity
        /// </summary>
        /// <param name="deviceId">Device Id</param>
        /// <param name="temperature">temperature</param>
        /// <param name="humidity">Humidity</param>
        /// <param name="localTime">Local time</param>
        /// <param name="isForward">is forward or backard</param>
        /// <param name="ticks">Number of ticks</param>
        public SensorHatEntity(string deviceId, int temperature, int humidity, DateTime localTime, bool isForward, long ticks)
        {
            PartitionKey = deviceId;
            RowKey = GenerateRowKey(isForward, ticks);
            Temperature = temperature;
            Humidity = humidity;
            LocalTime = localTime;
        }

        /// <summary>
        /// Generates the row key based on the ticks and the direction
        /// </summary>
        /// <param name="latestFirst"></param>
        /// <param name="ticks"></param>
        /// <returns></returns>
        internal static string GenerateRowKey(bool latestFirst, long ticks)
        {
            if (latestFirst)
            {
                var reverseTicks = DateTime.MaxValue.Ticks - ticks;
                return string.Format("{0}{1:D19}", LatestPrefix, reverseTicks);
            }
            else
            {
                return string.Format("{0}{1:D19}", OldestPrefix, ticks);
            }
        }

        /// <summary>
        /// Temperature
        /// </summary>
        public int Temperature { get; set; }

        /// <summary>
        /// Humidity
        /// </summary>
        public int Humidity { get; set; }

        /// <summary>
        /// Local time
        /// </summary>
        public DateTime LocalTime { get; set; }

        #region constants
        private const string LatestPrefix = "latest_";
        private const string OldestPrefix = "oldest_";
        #endregion
    }
}