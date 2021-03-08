using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Forum_API.Cores.WebSockets.Handler
{
    public class ShoutBoxHandler : WebSocketHandler
    {
        public ShoutBoxHandler()
        {
            this.PreKey = "SHOUT_BOX_";
        }
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            await Task.CompletedTask;
        }
    }
}
