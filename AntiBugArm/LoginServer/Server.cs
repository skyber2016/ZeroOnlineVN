using Core;
using Core.Utils;
using Renci.SshNet;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace LoginServer
{
    public static class Server
    {

        public static IDictionary<string, Client> Users;
        public static SshClient SshClient { get; set; }
        private static SimpleTcpServer _server { get; set; }
        private static readonly Logging Logging = Program.Logging;
        public static void Start()
        {
            var settings = Settings.GetSettings();
            Users = new Dictionary<string, Client>();
            _server = TcpService.GetServer();
            _server.ClientConnected += Server_ClientConnected;
            _server.ClientDisconnected += Server_ClientDisconnected;
            _server.DataReceived += Server_DataReceived;
            _server.Start(settings.PortLoginMid);
            var pathToPrivateKey = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "zr-ssh");
            var username = "root";
            var ip = settings.IpMid;
            var pk = new PrivateKeyFile(pathToPrivateKey);
            var keyFiles = new[] { pk };
            var methods = new List<AuthenticationMethod>() { new PrivateKeyAuthenticationMethod(username, keyFiles) };
            var con = new ConnectionInfo(ip, username, methods.ToArray());
            SshClient = new SshClient(con);
            Console.WriteLine($"Server started on {settings.PortLoginMid}");
        }

        private static void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            var sessionId = e.TcpClient.GetSessionId();
            if (Users.TryGetValue(sessionId, out Client client))
            {
                Task.Run(() =>
                {
                    client.GameSendDataToMid(sender, e);
                });
            }
            
        }

        private static void Server_ClientDisconnected(object sender, System.Net.Sockets.TcpClient e)
        {
            var sessionId = e.GetSessionId();
            if (Users.TryGetValue(sessionId, out Client client))
            {
                Logging.Write()($"Disposing client {sessionId}");
                client.Dispose();
                Users.Remove(sessionId);
            }
        }

        private static void Server_ClientConnected(object sender, System.Net.Sockets.TcpClient e)
        {
            var ip = e.GetIP();
            try
            {
                var sessionId = e.GetSessionId();
                var client = new Client(e, ()=>
                {
                    if (Users.TryGetValue(sessionId, out Client c))
                    {
                        Logging.Write()("Removing connection...");
                        Users.Remove(sessionId);
                    }
                });
                Users.Add(e.GetSessionId(), client);
                client.Listen();
            }
            catch (Exception ex)
            {
                Logging.Write()($"[{ip}] {ex.GetBaseException().Message}");
            }
        }
    }
}
