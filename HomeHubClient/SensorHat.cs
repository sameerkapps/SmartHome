////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using HomeHub.Model;
using HomeHubClient.Utils;

namespace HomeHubClient
{
    public class SensorHat : AuthenticatedClientBase
    {
        /// <summary>
        /// Constructor to initialize the base address and the token
        /// </summary>
        /// <param name="baseAddress">base address</param>
        /// <param name="authKey">Authentication Key</param>
        public SensorHat(string baseAddress, string authKey)
            :base(baseAddress, authKey)
        {
            _authKey = authKey;
        }

        /// <summary>
        /// Method to add data to the table (= POST)
        /// </summary>
        /// <param name="deviceId">Device Id</param>
        /// <param name="temperature">Temperature</param>
        /// <param name="humidity">Humidity</param>
        public async Task AddData(string deviceId, int temperature, int humidity)
        {
            var sensorHatModel = new SensorHatModel();
            sensorHatModel.DeviceId = deviceId;
            sensorHatModel.Humidity = humidity;
            sensorHatModel.Temperature = temperature;
            sensorHatModel.TimeStamp = DateTime.Now;

            // convert it to JSON
            var modelJson = JsonConvert.SerializeObject(sensorHatModel);

            // add model to the reuest as content with the right content type
            HttpContent content = new StringContent(modelJson);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("SensorHatDataAddKey", _authKey);

            // make that call
            HttpResponseMessage response = await Client.PostAsync(_baseAddress + "/api/SensorHat/AddData/", content);

            // if the call is not successful, throw exception
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Gets the data for the given device for the last few hours
        /// </summary>
        /// <param name="deviceId">Device Id</param>
        /// <param name="duration">Duration in hours</param>
        /// <returns></returns>
        public async Task<List<SensorHatModel>> GetData(string deviceId, int duration)
        {
            // e.g API
            // http://localhost:20090/api/SensorHat/List?deviceId=Home_LivingRoom&hrs=24
            var url = _baseAddress + $"/api/SensorHat/List?deviceId={deviceId}&hrs={duration}";

            // get the data
            HttpResponseMessage response = await Client.GetAsync(url);

            // if the result is successful
            // convert to list of strings and return it
            if (response.StatusCode.IsSuccessful())
            {
                var result = await response.Content.ReadAsStringAsync();
                var weatherData = JsonConvert.DeserializeObject<List<SensorHatModel>>(result);

                // convert date to LocalTime
                foreach (var data in weatherData)
                {
                    data.TimeStamp = data.TimeStamp.ToLocalTime();
                }
                return weatherData;
            }

            // if the call fails throw exception with reason
            throw new Exception(response.ReasonPhrase);
        }

        private readonly string _authKey;
    }
}
