# SignalR

透過網頁聊天室的範例練習 SignalR

## nuget 安裝 SignalR 套件

```shell
install-package Microsoft.AspNet.SignalR
```

## 設定 OWIN

建立 OWIN 啟動類別

```csharp
[assembly: OwinStartup(typeof(SingalRDemo.Startup))]
namespace SingalRDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
```

## 建立 server 端的 Hub

繼承`Microsoft.AspNet.SignalR.Hub`，並建立自訂的方法，例如當 Client 端發送資料給 Server 端，Server 端應如何處理

```csharp
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
}
```

## Client 端頁面 javascript 程式

範例採用聊天室，送出訊息給 Hub 再由 Hub 傳遞給每一個 Client，而其他的 Client 接到資料後要可以將訊息呈現出來

```html
<body>
    <input id="msg" value=""/>
    <input id="send" type="button" value="Send"/>
    <hr />
    <h3>Chat Message</h3>
    <ul id="room"></ul>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js'></script><!--需要先載入jQuery-->
    <script src='https://cdnjs.cloudflare.com/ajax/libs/signalr.js/2.3.0/jquery.signalR.min.js'></script>
    <script src="/signalr/hubs"></script><!--指向根目錄的/signalr/hubs-->
    <script src="chat.js"></script>
</body>
```

```javascript
// chat.js
let chat = $.connection.chatHub
let $sendBtn = $('#send')
let $msgDom = $('#msg')
let $room = $('#room')

// 提供給 Hub 呼叫，將傳來的文字顯示在畫面上
chat.client.addMessage = msg => $room.append(`<li>${msg}</li>`)

// 定義 Client 端送出訊息事件，呼叫 Hub 的 sendMessage 方法
let sendMsgHandler = () => {
  chat.server.sendMessage($msgDom.val())
  $msgDom.val('')
}

// 與 Hub 的連線完成之後，才綁定送出按鈕的事件
$.connection.hub.start().done(function() {
  $sendBtn.on('click', sendMsgHandler)
})
```

## WinForm 也加入聊天室

需要指定 Winform 要跟哪個 Hub 互動，這個部分就再 form 一開始的時候先指定，所以先宣告兩個 private 變數存放`HubConnection` 以及 `IHubProxy`

連線開始需要指定 SignalR 的網址，另外我們也會希望再連線收到資料的時候進行處理，因此在 HubConnection 的 Received 加入委派來處理

先將收到的字串轉為 dynamic 物件，範例如下，在依據呼叫的 Hub 名稱、方法名稱或內容來做其他處理

## Hub 傳遞的 Json 格式

```csharp
public void SendMessage(string msg)
{
    Clients.All.addMessage(msg);
}

public void ShowInfo()
{
    Clients.All.showSomething();
}
```

```json
{
  "H": "ChatHub",
  "M": "addMessage",
  "A": ["小叮噹加入了聊天室"]
}
```

```json
{
  "H": "ChatHub",
  "M": "showSomething",
  "A": []
}
```

# 參考資料

1. http://weisnote.blogspot.com/2012/08/signalr-webform-winform.html
1. https://docs.microsoft.com/zh-tw/aspnet/signalr/overview/getting-started/tutorial-getting-started-with-signalr
1. https://code.msdn.microsoft.com/SignalR-Getting-Started-b9d18aa9
1. https://dotblogs.com.tw/hatelove/archive/2012/07/01/signalr-introduction-about-realtime-website.aspx
