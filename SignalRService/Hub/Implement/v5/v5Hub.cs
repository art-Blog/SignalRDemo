using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRService.Hub.Implement.v5
{
    [HubName("v5hub")]
    public class V5Hub : Hub<IClient>
    {
        private static readonly ConnectionMapping<string> Connections =
            new ConnectionMapping<string>();

        /// <summary>
        /// 傳遞訊息給某人
        /// </summary>
        /// <param name="userId">要傳遞的對象</param>
        /// <param name="msg">訊息內容</param>
        public void SendPrivateMsg(string userId, string msg)
        {
            foreach (var connectionId in Connections.GetConnections(userId))
            {
                Clients.Client(connectionId).Received($"{msg} at {DateTime.Now:f} By Connections");
            }
        }

        /// <summary>
        /// 傳送訊息給Hub的所有人
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void Send(string msg)
        {
            var onlineCount = Connections.Count;
            Clients.All.Received($"{msg} at {DateTime.Now:f} 線上人數：{onlineCount}");
        }

        public override Task OnConnected()
        {
            var userId = Context.QueryString["id"];
            Connections.Add(userId, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.QueryString["id"];
            Connections.Remove(userId, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            var userId = Context.QueryString["id"];
            // 如果該使用者的ClientId不在清單內，就加入
            if (!Connections.GetConnections(userId).Contains(Context.ConnectionId))
            {
                Connections.Add(userId, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}