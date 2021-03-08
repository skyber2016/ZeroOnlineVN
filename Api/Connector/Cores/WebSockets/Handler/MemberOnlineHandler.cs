using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Forum_API.Cores.WebSockets.Handler
{
    public class MemberOnlineHandler : WebSocketHandler
    {
        public MemberOnlineHandler()
        {
            this.PreKey = "MEMBER_ONLINE_";
        }

        public override Task OnConnected(WebSocket socket, HttpContext context)
        {
            var key = Guid.NewGuid().ToString();
            if (context.Request.Query.ContainsKey("Authorization"))
            {
                key = context.Request.Query["Authorization"].ToString();
            }
            WebSocketConnectionManager.AddSocket(socket, PreKey + key);
            return this.SendMessageToAllAsync(string.Empty);
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            await base.OnDisconnected(socket);
            await this.SendMessageToAllAsync(string.Empty);
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            await Task.CompletedTask;
        }
    }
}
