using System;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRService.Hub
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