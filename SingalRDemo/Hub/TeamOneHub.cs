using System;
using Microsoft.AspNet.SignalR.Hubs;

namespace SingalRDemo.Hub
{
    [HubName("team1")]
    public class TeamOneHub : Microsoft.AspNet.SignalR.Hub
    {
        public void Send(string msg)
        {
            Clients.All.received($"{msg} at {DateTime.Now:f}");
        }
    }
}