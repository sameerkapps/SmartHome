////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;

namespace SmartHomePiApp
{
    /// <summary>
    /// settings for the app
    /// </summary>
    internal static class Settings
    {
        /// <summary>
        /// Base of the cloud
        /// </summary>
        internal static string CloudBaseAddress
        {
            get;
        } = "http://localhost:53912/"; // If you want to run in the cloud, update it with "http://YOURSITE.azurewebsites.net/";

        /// <summary>
        /// Validation Key header
        /// </summary>
        internal static string ValidationHeaderKey
        {
            get;
        } = "YOUR KEY";
    }
}
