using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using SignalRService.Provider;

[assembly: OwinStartup(typeof(SignalRService.Startup))]

namespace SignalRService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 註冊自訂的使用者ID Provider規則
            var idProvider = new CustomUserIdProvider();
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration { };
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}