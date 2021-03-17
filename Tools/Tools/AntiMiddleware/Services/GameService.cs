using API.Configurations;
using API.Cores;
using API.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MiddlewareTCP.command;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace MiddlewareTCP.Entities
{
    public class GameService : IDisposable
    {
        public string SessionId { get; set; }
        private SimpleTcpClient MidClient { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }
        private IDictionary<string, byte[]> Cache { get; set; }
        private string Username { get; set; }
        private string IP { get; set; }
        private IOptions<AppSettings> AppSettings
        {
            get
            {
                return this.UnitOfWork.AppSettings;
            }
        }
        private TcpClient Game { get; set; }
        private IGeneralService<LoginHistoryEntity> LoginHistoryService { get; set; }
        public GameService(IUnitOfWork unitOfWork)
        {
            try
            {
                this.LoginHistoryService = unitOfWork.GetInstance<IGeneralService<LoginHistoryEntity>>();
                this.Cache = new Dictionary<string, byte[]>();
                this.SessionId = DateTime.Now.Ticks.ToString("X2");
                this.UnitOfWork = unitOfWork;
                this.MidClient = new SimpleTcpClient();
                this.MidClient.DataReceived += ServerToMid_Receiver;
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
            
        }

        public void GameToMid_Disconnected(object sender, TcpClient e)
        {
            this.Dispose();
        }

        private void WriteError(Exception ex)
        {
            if(ex.InnerException != null)
            {
                this.WriteError(ex.InnerException);
            }
            this.UnitOfWork.Logger.Error("GAME:" + ex.Message);
        }

        public void GameToMid_Data(object sender, Message e)
        {
            try
            {
                var data = e.Data.Split();
                if (e.Data.GetPacketType(2).vnEquals(PacketContants.JoinGameRequest))
                {
                    var secretKey = e.Data.Skip(8).Take(4).ToArray().Split();
                    if (PacketContants.SecretKey.ContainsKey(secretKey))
                    {
                        var username = PacketContants.SecretKey[secretKey];
                        this.UnitOfWork.Logger.Info($"[{e.TcpClient.GetIP()}] [{username}] login success");
                        this.Username = username;
                        this.LoginHistoryService.AddAsync(new LoginHistoryEntity
                        {
                            Username = username,
                            LoginTime = DateTime.Now,
                            SessionId = this.SessionId
                        }).WaitTask();
                    }
                }
                if (e.Data.GetPacketType(2).vnEquals(PacketContants.ARM))
                {
                    var key = "ChangeARM" + this.SessionId + DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (!this.Cache.ContainsKey(key))
                    {
                        this.Cache.Clear();
                        this.Cache[key] = e.Data;
                    }
                    else
                    {
                        this.UnitOfWork.Logger.Error($"[{this.IP}] [{this.Username}] pendding change arm");
                        return;
                    }
                }
                if (this.MidClient.TcpClient.Connected)
                {
                    this.MidClient.Write(e.Data);
                }
                else
                {
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.WriteError(ex);
            }
            
        }

        public void GameToMid_Connected(object sender, TcpClient e)
        {
            try
            {
                this.Game = e;
                this.IP = this.Game.GetIP();
                this.MidClient.Connect(this.AppSettings.Value.IpServer, this.AppSettings.Value.PortGameServer);
            }
            catch (Exception ex)
            {
                this.Game.Close();
                this.Game.Dispose();
                this.WriteError(ex);
            }
            
        }

        public void ServerToMid_Receiver(object sender, Message e)
        {
            try
            {
                if (!this.Game.Connected)
                {
                    return;
                }
                this.Game.Client.Send(e.Data);
            }
            catch (Exception ex)
            {
                this.WriteError(ex);
            }
            
        }

        public void Dispose()
        {
            try
            {
                this.UnitOfWork.Logger.Info($"[{this.IP}] [{this.Username}] logout");
                var currentHistory = this.LoginHistoryService.SingleBy(new { session_id = this.SessionId }).WaitTask();
                if (currentHistory != null)
                {
                    currentHistory.LogoutTime = DateTime.Now;
                    this.LoginHistoryService.UpdateAsync(currentHistory).WaitTask();
                }
                this.Game.Close();
                this.Game.Dispose();
                this.MidClient.Disconnect();
                this.MidClient.Dispose();
            }
            catch (Exception ex)
            {
                this.WriteError(ex);
            }
            
        }
    }
}
