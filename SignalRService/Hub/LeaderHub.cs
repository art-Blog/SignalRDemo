using System;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRService.Hub
{
    [HubName("leader")]
    public class LeaderHub : Microsoft.AspNet.SignalR.Hub<IClient>
    {
        public void Send(string msg)
        {
            Clients.All.Received($"{msg} at {DateTime.Now:f}");
        }
    }
}