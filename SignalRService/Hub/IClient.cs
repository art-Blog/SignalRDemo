using System.Threading.Tasks;

namespace SignalRService.Hub
{
    public interface IClient
    {
        Task Received(string message);
    }
}