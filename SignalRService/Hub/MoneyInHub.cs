using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRService.Hub
{
    [HubName("chatHub")]
    public class ChatHub : Microsoft.AspNet.SignalR.Hub
    {
        private const string TeamName = "team";

        public override Task OnConnected()
        {
            var team = Context.QueryString[TeamName];
            return string.IsNullOrEmpty(team) ? base.OnConnected() : JoinRoom(team);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var team = Context.QueryString[TeamName];
            return string.IsNullOrEmpty(team) ? base.OnDisconnected(stopCalled) : LeaveRoom(team);
        }

        public Task JoinRoom(string roomName)
        {
            var username = Context.QueryString["name"];
            Groups.Add(Context.ConnectionId, roomName);
            return Clients.Group(roomName).addMessage($"{username} 加入 {roomName} 聊天室");
        }

        public Task LeaveRoom(string roomName)
        {
            var username = Context.QueryString["name"];
            Clients.Group(roomName).addMessage($"{username} 離開 {roomName} 聊天室");
            return Groups.Remove(Context.ConnectionId, roomName);
        }

        /// <summary>
        /// 傳遞訊息給所有client
        /// </summary>
        /// <param name="msg">聊天訊息</param>
        public void SendMessage(string msg)
        {
            var team = Context.QueryString[TeamName];
            if (string.IsNullOrEmpty(team))
            {
                Clients.All.addMessage($"{msg} at {DateTime.Now.ToShortDateString()}");
            }
            else
            {
                Clients.Group(team).addMessage($"[{team}]{msg}");
            }
        }

        public void ShowInfo()
        {
            Clients.All.showSomething();
        }
    }
}