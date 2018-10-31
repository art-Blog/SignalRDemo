using System;
using Microsoft.AspNet.SignalR.Hubs;

namespace SingalRDemo.Hub
{
    [HubName("team2")]
    public class TeamTwoHub : Microsoft.AspNet.SignalR.Hub
    {
        public void Send(string msg)
        {
            Clients.All.received($"{msg} at {DateTime.Now:f}");
        }
    }
}