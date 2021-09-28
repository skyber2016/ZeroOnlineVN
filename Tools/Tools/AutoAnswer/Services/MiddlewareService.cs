using API.Configurations;
using API.Cores;
using Microsoft.Extensions.Options;
using AutoAnswer.Entities;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace AutoAnswer.Services
{
    public class MiddlewareService : IDisposable
    {
        public SimpleTcpServer LoginServer { get; set; }
        public SimpleTcpServer GameServer { get; set; }
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
            this.UnitOfWork.Logger.Info($"LOGIN SERVER STARTED ON {unitOfWork.AppSettings.Value.PortLoginMid}");
            this.GameServer = new SimpleTcpServer();
            this.GameServer.DataReceived += GameServer_ReceiveData;
            this.GameServer.ClientConnected += GameServer_ClientConnected;
            this.GameServer.ClientDisconnected += GameServer_ClientDisconnected;
            this.GameServer.Start(unitOfWork.AppSettings.Value.PortGameMid);
            this.UnitOfWork.Logger.Info($"GAME SERVER STARTED ON {unitOfWork.AppSettings.Value.PortGameMid}");
        }

        private void GameServer_ClientDisconnected(object sender, TcpClient e)
        {
            var ip = e.GetIP();
            try
            {
                this.UnitOfWork.Logger.Info($"[MiddlewareService] [GameServer_ClientDisconnected] [LoginUsers available: {this.LoginUsers.Count}]");
                var sessionId = e.GetSessionId();
                var user = this.GameUsers[sessionId];
                user.Dispose();
                this.GameUsers.Remove(sessionId);
                this.UnitOfWork.Logger.Info($"[{ip}] [Middleware service] [Remove {sessionId} from LoginUsers]");
                this.UnitOfWork.Logger.Info($"[MiddlewareService] [GameServer_ClientDisconnected] [LoginUsers available: {this.LoginUsers.Count}]");

            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[MiddlewareService] [GameServer_ClientDisconnected] [{ip}] [Throw Exception]");
                this.WriteError(ex);
            }
            
        }
        private void WriteError(Exception ex)
        {
            if (ex.InnerException != null)
            {
                WriteError(ex.InnerException);
            }
            this.UnitOfWork.Logger.Error("LOGIN: " + ex.Message);
            this.UnitOfWork.Logger.Error("LOGIN: " + ex.StackTrace);
        }
        private void GameServer_ClientConnected(object sender, TcpClient e)
        {
            var ip = e.GetIP();
            try
            {
                var user = new GameService(this.UnitOfWork);
                this.GameUsers[e.GetSessionId()] = user;
                user.GameToMid_Connected(sender, e);
                this.UnitOfWork.Logger.Info($"{ip} connected");
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[MiddlewareService] [GameServer_ClientConnected] [{ip}] [Throw Exception]");
                this.WriteError(ex);
            }
        }

        private void GameServer_ReceiveData(object sender, Message e)
        {
            var ip = e.TcpClient.GetIP();
            try
            {
                if (!this.GameUsers.ContainsKey(e.TcpClient.GetSessionId()))
                {
                    this.UnitOfWork.Logger.Error($"[MiddlewareService] [GameServer_ReceiveData] [{ip}] [Not found user]");
                    return;
                }
                var user = this.GameUsers[e.TcpClient.GetSessionId()];
                user.GameToMid_Data(sender, e);
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[MiddlewareService] [GameServer_ReceiveData] [{ip}] [Throw Exception]");
                this.WriteError(ex);
            }
            
        }

        private void LoginServer_Disconnected(object sender, TcpClient e)
        {
            try
            {
                this.UnitOfWork.Logger.Info($"[MiddlewareService] [LoginServer_Disconnected] [LoginUsers available: {this.LoginUsers.Count}]");

                var ip = e.GetIP();
                var sessionId = e.GetSessionId();
                this.UnitOfWork.Logger.Info($"{e.GetIP()} disconnected");
                var user = this.LoginUsers[sessionId];
                user.Dispose();
                this.LoginUsers.Remove(sessionId);
                this.UnitOfWork.Logger.Info($"[{ip}] [Middleware service] [Remove {sessionId} from LoginUsers]");
                this.UnitOfWork.Logger.Info($"[MiddlewareService] [LoginServer_Disconnected] [LoginUsers available: {this.LoginUsers.Count}]");
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[MiddlewareService] [Throw Exception]");
                this.WriteError(ex);
            }
        }

        private void LoginServer_DataReceived(object sender, Message e)
        {
            var ip = e.TcpClient.GetIP();
            try
            {
                var user = this.LoginUsers[e.TcpClient.GetSessionId()];
                user.GameToMid_DataReceiver(sender, e);
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[LoginServer_DataReceived] [{ip}] [Throw Exception]");
                this.WriteError(ex);
            }
            
        }

        private void LoginServer_ClientConnected(object sender, TcpClient e)
        {
            var ip = e.GetIP();
            try
            {
                var user = new LoginService(this.UnitOfWork);
                this.LoginUsers[e.GetSessionId()] = user;
                this.UnitOfWork.Logger.Info($"{e.GetIP()} connected");
                user.GameToMid_Connected(sender, e);
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[MiddlewareService] [LoginServer_ClientConnected] [{ip}] [Throw Exception]");
                this.WriteError(ex);
            }
            
        }

        public void Dispose()
        {
            try
            {
                this.UnitOfWork.Logger.Info("SERVER STOPPING");
                this.UnitOfWork.Logger.Info($"Begin disconnect {this.GameUsers.Count} user");
                foreach (var item in this.GameUsers)
                {
                    item.Value.Dispose();
                }
                this.LoginUsers.Clear();
                this.GameUsers.Clear();
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[MiddlewareService] [Dispose] [Throw Exception]");
                this.WriteError(ex);
            }
            
        }
    }
}
