using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace SingalRDemo.Hub
{
    [HubName("oneuser")]
    public class OneUserHub : Microsoft.AspNet.SignalR.Hub<IClient>
    {
        public override Task OnConnected()
        {
            var id = Context.QueryString["id"];
            Groups.Add(Context.ConnectionId, id);

            return base.OnConnected();
        }

        public void SendPrivateMsg(string userId, string msg)
        {
            Clients.Group(userId).Received(msg);
        }

        public void Send(string msg)
        {
            Clients.All.Received($"{msg} at {DateTime.Now:f}");
        }
    }
}