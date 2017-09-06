using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SmartHomeRESTAPI.App_Start.Startup))]
namespace SmartHomeRESTAPI.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}