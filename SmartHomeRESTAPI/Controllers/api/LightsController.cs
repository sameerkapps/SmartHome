////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.Web.Http;

using Microsoft.AspNet.SignalR;

using HomeHub.Model;

using SmartHomeRESTAPI.Attributes;
using SmartHomeRESTAPI.Hub;

namespace SmartHomeRESTAPI.Controllers.api
{
    /// <summary>
    /// Controller for the lights
    /// </summary>
    public class LightsController : ApiController
    {
        // PUT api
        /// <summary>
        /// Access allowed only there is a valid key
        /// Future - To be later replaced with Azure AD
        /// </summary>
        /// <param name="lightSwitch">Model</param>
        [CustomKeyAuthorize] // allow only valid users
        [ActionName("switch")]
        [HttpPut]
        public void Put([FromBody]LightSwitchModel lightSwitch)
        {
            var homeContext = GlobalHost.ConnectionManager.GetHubContext<SwitchHub>();

            homeContext.Clients.All.FlipSwitch(lightSwitch.Id, lightSwitch.IsOn);
        }
    }
}
