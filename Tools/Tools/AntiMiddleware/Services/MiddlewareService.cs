using API.Configurations;
using API.Cores;
using Microsoft.Extensions.Options;
using MiddlewareTCP.Entities;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace MiddlewareTCP.Services
{
    public class MiddlewareService : IDisposable
    {
        private SimpleTcpServer LoginServer { get; set; }
        private SimpleTcpServer GameServer { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }
        private IDictionary<string, LoginService> LoginUsers { get; set; }
        private IDictionary<string, GameService> GameUsers { get; set; }
        public MiddlewareService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            this.LoginUsers = new Dictionary<string, LoginService>();
            this.GameUsers = new Dictionary<string, GameService>();
            this.LoginServer = new SimpleTcpServer();
            this.LoginServer.ClientConnected += LoginServer_ClientConnected;
            this.LoginServer.DataReceived += LoginServer_DataReceived;
            this.LoginServer.ClientDisconnected += LoginServer_Disconnected;
            this.LoginServer.Start(unitOfWork.AppSettings.Value.PortLoginMid);

            this.GameServer = new SimpleTcpServer();
            this.GameServer.DataReceived += GameServer_ReceiveData;
            this.GameServer.ClientConnected += GameServer_ClientConnected;
            this.GameServer.ClientDisconnected += GameServer_ClientDisconnected;
            this.GameServer.Start(unitOfWork.AppSettings.Value.PortGameMid);
        }

        private void GameServer_ClientDisconnected(object sender, TcpClient e)
        {
            var user = this.GameUsers[e.GetSessionId()];
            user.Dispose();
        }

        private void GameServer_ClientConnected(object sender, TcpClient e)
        {
            var user = new GameService(this.UnitOfWork);
            this.GameUsers[e.GetSessionId()] = user;
            user.GameToMid_Connected(sender, e);
            this.UnitOfWork.Logger.Info($"{e.GetIP()} connected");
        }

        private void GameServer_ReceiveData(object sender, Message e)
        {
            if (!this.GameUsers.ContainsKey(e.TcpClient.GetSessionId()))
            {
                return;
            }
            var user = this.GameUsers[e.TcpClient.GetSessionId()];
            user.GameToMid_Data(sender, e);
        }

        private void LoginServer_Disconnected(object sender, TcpClient e)
        {
            var user = this.LoginUsers[e.GetSessionId()];
            user.Dispose();
        }

        private void LoginServer_DataReceived(object sender, Message e)
        {
            var user = this.LoginUsers[e.TcpClient.GetSessionId()];
            user.GameToMid_DataReceiver(sender, e);
        }

        private void LoginServer_ClientConnected(object sender, TcpClient e)
        {
            var user = new LoginService(this.UnitOfWork);
            this.LoginUsers[e.GetSessionId()] = user;
            this.UnitOfWork.Logger.Info($"{e.GetIP()} connected");
            user.GameToMid_Connected(sender, e);
        }

        public void Dispose()
        {
            this.LoginUsers.Clear();
            this.GameUsers.Clear();
        }
    }
}
