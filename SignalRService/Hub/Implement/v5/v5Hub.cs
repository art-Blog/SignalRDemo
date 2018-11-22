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
        /// �ǻ��T�����Y�H
        /// </summary>
        /// <param name="userId">�n�ǻ�����H</param>
        /// <param name="msg">�T�����e</param>
        public void SendPrivateMsg(string userId, string msg)
        {
            foreach (var connectionId in Connections.GetConnections(userId))
            {
                Clients.Client(connectionId).Received($"{msg} at {DateTime.Now:f} By Connections");
            }
        }

        /// <summary>
        /// �ǰe�T����Hub���Ҧ��H
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void Send(string msg)
        {
            var onlineCount = Connections.Count;
            Clients.All.Received($"{msg} at {DateTime.Now:f} �u�W�H�ơG{onlineCount}");
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
            // �p�G�ӨϥΪ̪�ClientId���b�M�椺�A�N�[�J
            if (!Connections.GetConnections(userId).Contains(Context.ConnectionId))
            {
                Connections.Add(userId, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}