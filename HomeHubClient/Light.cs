/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
  * License: MIT License
***********************************************************************************************/
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using HomeHub.Model;

namespace HomeHubClient
{
    /// <summary>
    ///  client class to expose WebAPIs for light
    /// </summary>
    public class Light : AuthenticatedClientBase
    {
        /// <summary>
        /// Constructor to initialize the base address and the token
        /// </summary>
        /// <param name="baseAddress">base address</param>
        /// <param name="authKey">Authentication Key</param>
        public Light(string baseAddress, string authKey)
            :base(baseAddress, authKey)
        {
            _authKey = authKey;
        }

        /// <summary>
        /// Makes a call to WebAPI to turn light on/off
        /// </summary>
        /// <param name="lightId">Id of the light</param>
        /// <param name="turnOn">on/off flag</param>
        /// <returns></returns>
        public async Task SwitchLight(string lightId, bool turnOn)
        {
            // validate parameters
            if (string.IsNullOrEmpty(lightId))
            {
                throw new ArgumentNullException(nameof(lightId));
            }

            // Create the model to be transmitted
            var lightswichModel = new LightSwitchModel() { Id = lightId, IsOn = turnOn };
            // convert it to JSON
            var modelJson = JsonConvert.SerializeObject(lightswichModel);

            // add model to the reuest as content with the right content type
            HttpContent content = new StringContent(modelJson);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add("SensorHatDataAddKey", _authKey);

            // make that call
            HttpResponseMessage response = await Client.PutAsync(_baseAddress + "/api/lights/switch/", content);

            // if the call is not successful, throw exception
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        private readonly string _authKey;
    }
}
