////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

using HomeHub.Model;

namespace SmartHomeRESTAPI.AzureStorageDB
{
    /// <summary>
    /// Interface for the sensor hat
    /// </summary>
    public interface ISensorHatRepository
    {
        /// <summary>
        /// Add sensor data
        /// </summary>
        /// <param name="deviceId">Device Id</param>
        /// <param name="temperature">Temperature</param>
        /// <param name="humidity">Humidity</param>
        /// <param name="localTime">Local time</param>
        /// <returns></returns>
        bool AddSensorHatData(string deviceId, int temperature, int humidity, DateTime localTime);

        /// <summary>
        /// Gets the data as per the sorted order
        /// </summary>
        /// <param name="deviceId">Id of the device</param>
        /// <param name="startUTCTime">Start time in UTC</param>
        /// <param name="endUTCTime">End time in UTC</param>
        /// <param name="latestFirst">Order of the data</param>
        /// <returns></returns>
        List<SensorHatModel> GetSensorHatData(string deviceId, DateTime startUTCTime, DateTime endUTCTime, bool latestFirst);
    }
}
