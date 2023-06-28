using Core;
using Core.Utils;
using SimpleTCP;
using System;
using System.Collections.Generic;

namespace GameServer
{
    public static class Server
    {
        private static readonly Logging Logging = Program.Logging;
        private static IDictionary<string, Client> Users { get; set; }
        private static string _username { get; set; }
        public static void Starting(int port, string username)
        {

            try
            {
                Console.WriteLine($"username -> {username} port -> {port}");
                _username = username;
                Users = new Dictionary<string, Client>();
                var server = new SimpleTcpServer();
                Console.WriteLine($"username -> {username} port -> {port}");
                server.ClientConnected += Server_ClientConnected;
                server.ClientDisconnected += Server_ClientDisconnected;
                server.DataReceived += Server_DataReceived;
                server.Start(port);
                Console.WriteLine($"username -> {username} port -> {port}");
                Logging.Write()($"Server started on {port}");
                Console.WriteLine($"Server started on {port}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
                Console.WriteLine(ex.GetBaseException().StackTrace);
            }
        }

        private static void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            var sessionId = e.TcpClient.GetSessionId();
            if (Users.TryGetValue(sessionId, out Client client))
            {
                client.GameSendDataToMid(sender, e);
            }

        }

        private static void Server_ClientDisconnected(object sender, System.Net.Sockets.TcpClient e)
        {
            var sessionId = e.GetSessionId();
            if (Users.TryGetValue(sessionId, out Client client))
            {
                client.Dispose();
                Users.Remove(sessionId);
            }
            Environment.Exit(1);
        }

        private static void Server_ClientConnected(object sender, System.Net.Sockets.TcpClient e)
        {
            Program.Watch.Stop();
            var ip = e.GetIP();
            try
            {
                var client = new Client(e);
                client.Username = _username;
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
