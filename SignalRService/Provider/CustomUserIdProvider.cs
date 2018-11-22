using Microsoft.AspNet.SignalR;

namespace SignalRService.Provider
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            // 從QueryString中取得ID作為使用者的識別名稱
            var id = request.QueryString["id"];
            return string.IsNullOrWhiteSpace(id) ? string.Empty : id;
        }
    }
}