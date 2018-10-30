using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SingalRDemo.Startup))]

namespace SingalRDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}