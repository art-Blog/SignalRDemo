$.connection.hub.url = 'http://localhost:22641/signalr'

let $name = $('#name')
let $sendBtn = $('#send')
let $msgDom = $('#msg')
let $room = $('#room')
let $btnJoin = $('#btnJoin')
let $btnJoinMoneyIn = $('#btnJoinMoneyIn')
let $btnLeave = $('#btnLeave')
let $notice = $('#notice')
let $btnNotice = $('#btnNotice')

let team1Proxy = $.connection.chatHub

//================Client Method======================================
team1Proxy.client.addMessage = msg => $room.append(`<li>${msg}</li>`)

let sendMsgHandler = () => {
  let username = $name.val()
  let msg = $msgDom.val()
  team1Proxy.server.sendMessage(`${username}：${msg}`)
  $msgDom.val('')
}
let leaveRoomHandler = () => {
  $.connection.hub.stop()
}
let joinRoomHandler = () => {
  let roomName = 'moneyCome'
  let userName = $name.val()
  $.connection.hub.qs = { team: roomName, name: userName }
  $.connection.hub.start()
}
let joinRoomMoneyInHandler = () => {
  let roomName = 'moneyIn'
  let userName = $name.val()
  $.connection.hub.qs = { team: roomName, name: userName }
  $.connection.hub.start()
}

$sendBtn.on('click', sendMsgHandler) // 綁定聊天按鈕事件
$btnJoin.on('click', joinRoomHandler) // 加入聊天室
$btnJoinMoneyIn.on('click', joinRoomMoneyInHandler) // 加入聊天室
$btnLeave.on('click', leaveRoomHandler) //離開聊天室
