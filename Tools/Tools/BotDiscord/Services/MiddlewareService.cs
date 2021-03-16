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
        public MiddlewareService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            this.LoginUsers = new Dictionary<string, LoginService>();
            this.LoginServer = new SimpleTcpServer();
            this.LoginServer.ClientConnected += LoginServer_ClientConnected;
            this.LoginServer.DataReceived += LoginServer_DataReceived;
            this.LoginServer.ClientDisconnected += LoginServer_Disconnected;
            this.LoginServer.Start(unitOfWork.AppSettings.Value.PortLoginMid);
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
            user.GameToMid_Connected(sender, e);
            this.UnitOfWork.Logger.Info($"{e.GetIP()} connected");
        }

        public void Dispose()
        {
            this.LoginUsers.Clear();
        }
    }
}
