using System;
using Microsoft.AspNet.SignalR;

namespace SignalRService.Hub.Base
{
    public abstract class BaseHub : Hub<IClient>
    {
        /// <summary>
        /// 傳遞訊息給某人
        /// </summary>
        /// <param name="userId">要傳遞的對象</param>
        /// <param name="msg">訊息內容</param>
        public void SendPrivateMsg(string userId, string msg)
        {
            //透過使用者ID來針對該使用者相關聯的連接發送訊息
            //此處Clients.User()必須要先註冊自訂的IUserIdProvider，讓SignalR知道怎麼取得使用者Id
            Clients.User(userId).Received($"{msg} at {DateTime.Now:f} By User");
        }

        /// <summary>
        /// 傳送訊息給Hub的所有人
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void Send(string msg)
        {
            Clients.All.Received($"{msg} at {DateTime.Now:f} By All");
        }
    }
}