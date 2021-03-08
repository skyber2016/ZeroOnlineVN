using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Forum_API.Cores.WebSockets.Handler
{
    public class TopicHandler : WebSocketHandler
    {
        public TopicHandler()
        {
            this.PreKey = "TOPICS_";
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            await Task.CompletedTask;
        }
    }
}
