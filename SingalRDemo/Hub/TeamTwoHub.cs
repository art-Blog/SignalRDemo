using System;
using Microsoft.AspNet.SignalR.Hubs;

namespace SingalRDemo.Hub
{
    [HubName("team2")]
    public class TeamTwoHub : Microsoft.AspNet.SignalR.Hub<IClient>
    {
        public void Send(string msg)
        {
            Clients.All.Received($"{msg} at {DateTime.Now:f}");
        }
    }
}