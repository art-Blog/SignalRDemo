let $name = $('#name')
let $notice = $('#notice')
let $btnNotice = $('#btnNotice')
let $room = $('#room')

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
let leaveRoomHandler = () => $.connection.hub.stop()

let noticeHandler = () => {
  let notice = $notice.val()
  let userName = $name.val()

  let state = $.connection.hub.state
  if (state === 4) {
    // 未連線
    $.connection.hub.qs = { team: '', name: userName }
    $.connection.hub.start().done(function() {
      hub.server.sendMessage(`${userName}：${notice}`)
      $notice.val('')
    })
  } else {
    hub.server.sendMessage(`${userName}：${notice}`)
    $notice.val('')
  }
}

$btnNotice.on('click', noticeHandler) // 公告
