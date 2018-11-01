using System.Threading.Tasks;

namespace SingalRDemo.Hub
{
    public interface IClient
    {
        Task Received(string message);
    }
}