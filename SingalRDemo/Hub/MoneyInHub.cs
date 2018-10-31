using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace SingalRDemo.Hub
{
    [HubName("chatHub")]
    public class ChatHub : Microsoft.AspNet.SignalR.Hub
    {
        public override Task OnConnected()
        {
            // 先取得使用者組別
            var team = Context.QueryString["team"];

            // 將連線加入該組別
            return JoinRoom(team);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var team = Context.QueryString["team"];
            try
            {
                return LeaveRoom(team);
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                Clients.Group(team).addMessage(Context.User.Identity.Name + " Leave.");
            }
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
            // Clients.All.addMessage($"{msg} at {DateTime.Now.ToShortDateString()}");

            var team = Context.QueryString["team"];
            Clients.Group(team).addMessage($"[{team}]{msg}");
        }

        public void ShowInfo()
        {
            Clients.All.showSomething();
        }
    }
}