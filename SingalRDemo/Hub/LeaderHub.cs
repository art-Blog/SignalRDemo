using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;

namespace SingalRDemo.Hub
{
    [HubName("leader")]
    public class LeaderHub : Microsoft.AspNet.SignalR.Hub
    {
        public void Send(string msg)
        {
            Clients.All.received($"{msg} at {DateTime.Now:f}");
        }
    }
}