using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeApp
{
    internal static class Settings
    {
        internal static string CloudBaseAddress
        {
            get;
        } = "http://smarthomesvc.azurewebsites.net/"; // "http://localhost:53912/";  // TO REMOVE

        internal static string ValidationHeaderKey
        {
            get;
        } = "3CE46F14-183D-4F59-A464-2848981046F0";
    }
}
