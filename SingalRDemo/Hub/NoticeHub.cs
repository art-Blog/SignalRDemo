using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;

namespace SingalRDemo.Hub
{
    [HubName("notice")]
    public class NoticeHub : Microsoft.AspNet.SignalR.Hub<IClient>
    {
        public void Send(string msg)
        {
            Clients.All.Received($"{msg} at {DateTime.Now:f}");
        }
    }
}