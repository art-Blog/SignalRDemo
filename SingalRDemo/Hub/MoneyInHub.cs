namespace SingalRDemo.Hub
{
    public class ChatHub : Microsoft.AspNet.SignalR.Hub
    {
        /// <summary>
        /// 傳遞訊息給所有client
        /// </summary>
        /// <param name="msg">聊天訊息</param>
        public void SendMessage(string msg)
        {
            Clients.All.addMessage(msg);
        }

        public void ShowInfo()
        {
            Clients.All.showSomething();
        }
    }
}