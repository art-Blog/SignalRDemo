let $name = $('#name')
let $sendBtn = $('#send')
let $msgDom = $('#msg')
let $room = $('#room')
let $winform = $('#winform')
let $btnJoin = $('#btnJoin')
let $btnLeave = $('#btnLeave')

let hub = $.connection.chatHub

//================Client Method======================================
hub.client.addMessage = msg => $room.append(`<li>${msg}</li>`)
hub.client.showSomething = () => $room.append(`<li>正在查詢...</li>`)

let sendMsgHandler = () => {
  let username = $name.val()
  let msg = $msgDom.val()
  hub.server.sendMessage(`${username}：${msg}`)
  $msgDom.val('')
}
let showInfoHandler = () => hub.server.showInfo()
let leaveRoomHandler = () => {
  $.connection.hub.stop()
}
let joinRoomHandler = () => {
  let roomName = 'moneyCome'
  let userName = $name.val()
  $.connection.hub.qs = { team: roomName, name: userName }
  $.connection.hub.start()
}

$sendBtn.on('click', sendMsgHandler) // 綁定聊天按鈕事件
$winform.on('click', showInfoHandler) // 綁定特殊按鈕事件
$btnJoin.on('click', joinRoomHandler) // 加入聊天室
$btnLeave.on('click', leaveRoomHandler) //離開聊天室
