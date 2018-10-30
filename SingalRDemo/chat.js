let conn = $.connection.chatHub
let $name = $('#name')
let $sendBtn = $('#send')
let $msgDom = $('#msg')
let $room = $('#room')
let $winform = $('#winform')

// 提供給 Hub 呼叫，將傳來的文字顯示在畫面上
conn.client.addMessage = msg => $room.append(`<li>${msg}</li>`)
conn.client.showSomething = () => $room.append(`<li>正在查詢...</li>`)

// 定義 Client 端送出訊息事件，呼叫 Hub 的 sendMessage 方法
let sendMsgHandler = () => {
  let username = $name.text()
  let msg = $msgDom.val()

  conn.server.sendMessage(`${username}：${msg}`)
  $msgDom.val('')
}

let showInfo = () => conn.server.showInfo()

$.connection.hub.start().done(function() {
  // 綁定聊天按鈕事件
  $sendBtn.on('click', sendMsgHandler)

  // 綁定特殊按鈕事件
  $winform.on('click', showInfo)
})
