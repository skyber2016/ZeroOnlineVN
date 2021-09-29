using API.Configurations;
using API.Cores;
using AutoAnswer.Services.Interfaces;
using Microsoft.Extensions.Options;
using AutoAnswer.command;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using AutoAnswer.Services;

namespace AutoAnswer.Entities
{
    public class GameService : IDisposable
    {
        public string SessionId { get; set; }
        private SimpleTcpClient MidClient { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }
        private IDictionary<string, byte[]> Cache { get; set; }
        private string Username { get; set; } = string.Empty;
        private string IP { get; set; } = "NOIP";
        private IAtomService AtomService { get; set; }
        private IOptions<AppSettings> AppSettings
        {
            get
            {
                return this.UnitOfWork.AppSettings;
            }
        }
        private TcpClient Game { get; set; }
        public GameService(IUnitOfWork unitOfWork)
        {
            try
            {
                this.Cache = new Dictionary<string, byte[]>();
                this.SessionId = Guid.NewGuid().ToString();
                this.UnitOfWork = unitOfWork;
                this.MidClient = new SimpleTcpClient();
                this.MidClient.DataReceived += ServerToMid_Receiver;
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [Contructor] [{ex.Message}]");
                WriteError(ex);
            }
            
        }

        public void GameToMid_Disconnected(object sender, TcpClient e)
        {
            this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Disconnected] [Game disconnected]");
            this.Dispose();
        }

        private void WriteError(Exception ex)
        {
            if(ex.InnerException != null)
            {
                this.WriteError(ex.InnerException);
            }
            this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [WriteError] [{ex.Message}]");
            this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [WriteError] [{ex.StackTrace}]");
        }
        public void GameToMid_Data(object sender, Message e)
        {
            try
            {
                
                var data = e.Data.Split();
                this.AtomService.SetPartern(e.Data);
                if (this.MidClient.TcpClient.Connected)
                {
                    try
                    {
                        this.MidClient.Write(e.Data);
                    }
                    catch (Exception ex)
                    {
                        this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Data] [Dispose] [Mid client throw abort connection]");
                        this.WriteError(ex);
                        this.Dispose();
                        return;
                    }
                }
                else
                {
                    this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Data] [Dispose] [Client disconnected]");
                    this.Dispose();
                    return;
                }
                if (e.Data.GetPacketType(2).vnEquals(PacketContants.JoinGameRequest))
                {
                    this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [GameToMid_Data] [Begin join game request]");
                    var secretKey = e.Data.Skip(8).Take(4).ToArray().Split();
                    if (PacketContants.SecretKey.ContainsKey(secretKey))
                    {
                        var username = PacketContants.SecretKey[secretKey];
                        this.UnitOfWork.Logger.Info($"[{e.TcpClient.GetIP()}] [{username}] login success");
                        this.Username = username;
                    }
                    this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [GameToMid_Data] [End join game request]");
                }
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Data] [Throw Exception]");
                this.WriteError(ex);
            }
        }

        public void GameToMid_Connected(object sender, TcpClient e)
        {
            try
            {
                this.Game = e;
                this.AtomService = new AtomService(MidClient, Game, UnitOfWork);
                this.IP = this.Game.GetIP();
                try
                {
                    this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [GameToMid_Connected] [Begin Connect to server]");
                    this.MidClient.Connect(this.AppSettings.Value.IpServer, this.AppSettings.Value.PortGameServer);
                    this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [GameToMid_Connected] [End connect to server success]");
                }
                catch (Exception ex)
                {
                    this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Connected] [{ex.Message}]");
                    this.WriteError(ex);
                }
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [GameToMid_Connected] [{ex.Message}]");
                this.Game.Close();
                this.Game.Dispose();
                this.WriteError(ex);
            }
            
        }

        public void ServerToMid_Receiver(object sender, Message e)
        {
            try
            {
                if(this.UnitOfWork.AnswerService.IsQuestion(e.Data.vnClone()))
                {
                    var question = this.UnitOfWork.AnswerService.GetQuest(e.Data.vnClone());
                    if(question != null)
                    {
                        if(question.FinalAnswer != null)
                        {
                            this.MidClient.Write(question.FinalAnswer);
                            return;
                        }
                    }
                }
                var atoms = this.AtomService.GetAtoms(e.Data.vnClone());
                if (atoms.Any())
                {
                    this.AtomService.SetAtoms(atoms);
                    this.AtomService.UseAtom();
                }

                if (!this.Game.Client.Connected)
                {
                    this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [ServerToMid_Receiver] [Game not connected]");
                    this.Dispose();
                    return;
                }
                try
                {
                    this.Game.Client.Send(e.Data);
                }
                catch (Exception ex)
                {
                    this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [ServerToMid_Receiver] [Game throw abort connection]");
                    this.WriteError(ex);
                    this.Dispose();
                }

            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [ServerToMid_Receiver] [Throw exception]");
                this.WriteError(ex);
            }
        }


        public void Dispose()
        {
            try
            {
                this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [Dispose] [Begin Logout client]");
                this.Game.Close();
                this.Game.Dispose();
                this.MidClient.Disconnect();
                this.MidClient.Dispose();
                this.UnitOfWork.Logger.Info($"[{this.Username}] [GameService] [Dispose] [End Logout client]");
            }
            catch (Exception ex)
            {
                this.UnitOfWork.Logger.Error($"[{this.Username}] [GameService] [Dispose] [{ex.Message}]");
                this.WriteError(ex);
            }
            
        }
    }
}
