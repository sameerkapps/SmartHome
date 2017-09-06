////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Results;

using HomeHub.Model;

using SmartHomeRESTAPI.Attributes;
using SmartHomeRESTAPI.AzureStorageDB;

namespace SmartHomeRESTAPI.Controllers.api
{
    /// <summary>
    /// Controller for the sense hat
    /// </summary>
    public class SensorHatController : ApiController
    {
        /// <summary>
        /// Gets the data for the given device id in the last given hours
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="hrs"></param>
        /// <returns></returns>
        [ActionName("List")]
        [HttpGet]
        // GET: api/SensorHat/5
        public JsonResult<List<SensorHatModel>> Get(string deviceId, int hrs)
        {
            var now = DateTime.UtcNow;
            var repos = new SensorHatRepository();
            var list = repos.GetSensorHatData(deviceId, now, now.AddHours(-1 * hrs), true);

            return Json<List<SensorHatModel>>(list);
        }

        /// <summary>
        /// Adds data to the repository
        /// </summary>
        /// <param name="weatherData"></param>
        // POST: api/SensorHat
        [ActionName("AddData")]
        [CustomKeyAuthorize]
        public void Post([FromBody]SensorHatModel weatherData)
        {
            var repos = new SensorHatRepository();

            repos.AddSensorHatData(weatherData.DeviceId, weatherData.Temperature, weatherData.Humidity, weatherData.TimeStamp);
        }

        // PUT: api/SensorHat/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SensorHat/5
        public void Delete(int id)
        {
        }
    }
}
