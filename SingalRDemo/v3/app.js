﻿import * as tool from './common.js'
;(function() {
  let $sendBtn = $('#send')
  let $sendPrivateBtn = $('#sendPrivateBtn')
  let $msgDom = $('#msg')
  let $room = $('#room')
  let userId = $('#userId').val()
  // Data Binding to UI
  $('#name').val(data.name)
  $('#channel').text(data.channel.map(x => x.name).join('、'))

  for (let index = 0; index < data.channel.length; index++) {
    let currectChannelId = data.channel[index].id
    let currectProxy = tool.getProxy(currectChannelId)
    currectProxy.client.received = msg => $room.append(`<li>${msg}</li>`)
  }

  $.connection.hub.url = 'http://localhost:22641/signalr'
  $.connection.hub.qs = { id: data.id }

  $.connection.hub.start().done(function() {
    $sendBtn.on('click', function() {
      let currectProxy = tool.getProxy($('#channelId').val())
      let channelName = data.channel.find(
        x => x.id === parseInt($('#channelId').val(), 10)
      ).name

      currectProxy.server.send(`[${channelName}]${data.name}：${$msgDom.val()}`)
      $msgDom.val('')
    })

    $sendPrivateBtn.on('click', function() {
      // userId is a pk for user

      let currectProxy = tool.getProxy($('#channelId').val())
      currectProxy.server.sendPrivateMsg(
        userId,
        `[PM]${data.name}：${$msgDom.val()}`
      )
      $msgDom.val('')
    })
  })
})()
