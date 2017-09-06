////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SmartHomeRESTAPI.Attributes
{
    public class CustomKeyAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Static constructor to read values from config and assign them
        /// </summary>
        static CustomKeyAuthorizeAttribute()
        {
            SensorHatDataAddKey = ConfigurationManager.AppSettings.Get(nameof(SensorHatDataAddKey));
        }

        /// <summary>
        /// Checks, if the current user is as per authorization Level 1
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var currHeaders = HttpContext.Current.Request.Headers;
            if (currHeaders.AllKeys.Any((key) => string.Equals(key, nameof(SensorHatDataAddKey), StringComparison.InvariantCultureIgnoreCase)))
            {
                return string.Equals(currHeaders[nameof(SensorHatDataAddKey)], 
                                     SensorHatDataAddKey, 
                                     StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        private static readonly string SensorHatDataAddKey;
    }
}