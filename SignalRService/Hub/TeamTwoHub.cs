using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRService.Hub
{
    [HubName("team2")]
    public class TeamTwoHub : Microsoft.AspNet.SignalR.Hub<IClient>
    {
        public override Task OnConnected()
        {
            var id = Context.QueryString["id"];
            Groups.Add(Context.ConnectionId, id);

            return base.OnConnected();
        }

        /// <summary>
        /// 傳遞訊息給單一用戶組
        /// </summary>
        /// <param name="userId">要傳遞的對象</param>
        /// <param name="msg">訊息內容</param>
        public void SendPrivateMsg(string userId, string msg)
        {
            Clients.Group(userId).Received($"{msg} at {DateTime.Now:f}");
        }

        public void Send(string msg)
        {
            Clients.All.Received($"{msg} at {DateTime.Now:f}");
        }
    }
}