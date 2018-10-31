export const getProxy = channelId => {
  let id = parseInt(channelId, 10)
  switch (id) {
    case 0:
      return $.connection.team1
    case 1:
      return $.connection.team2
    case 2:
      return $.connection.leader
    case 3:
      return $.connection.notice
    default:
      return $.connection.team1
  }
}
