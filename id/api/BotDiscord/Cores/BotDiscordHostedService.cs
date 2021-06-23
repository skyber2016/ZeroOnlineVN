using API.Common;
using API.Configurations;
using API.Entities;
using API.Helpers;
using API.Services.Interfaces;
using BotDiscord;
using Discord;
using Discord.WebSocket;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Unity;

namespace API.Cores
{
    public class BotDiscordHostedService :  BackgroundService 
    {
        public IGeneralService<BotMessageEntity> BotMessageService { get; set; }
        public AppSettings AppSettings { get; set; }
        public ILoggerManager Logger { get; set; }
        public IGeneralService<AccountEntity> AccountService { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        private DiscordSocketClient Client { get; set; }
        public BotDiscordHostedService(
            IOptions<AppSettings> options,
            IUnitOfWork unitOfWork,
            ILoggerManager logger,
            IGeneralService<BotMessageEntity> botMessageService,
            IGeneralService<AccountEntity> accountService
            )
        {
            this.AppSettings = options.Value;
            this.UnitOfWork = unitOfWork;
            this.Logger = logger;
            this.BotMessageService = botMessageService;
            this.AccountService = accountService;
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            XmlDocument log4netConfig = new XmlDocument();

            using (var fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"))
            {
                log4netConfig.Load(fs);

                var repo = LogManager.CreateRepository(
                        Assembly.GetEntryAssembly(),
                        typeof(log4net.Repository.Hierarchy.Hierarchy));

                XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
            }
        }

        private bool ClientReady { get; set; }
        private async Task Client_Ready()
        {
            this.Logger.Info("BOT ARE READY");
            this.ClientReady = true;
            await Task.CompletedTask;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (this.Client == null)
                    {
                        this.Client = new DiscordSocketClient();
                        this.Client.Ready += async () => await this.Client_Ready();
                        this.Client.MessageReceived += Client_MessageReceived;
                        await this.Client.StartAsync();
                        await this.Client.LoginAsync(Discord.TokenType.Bot, this.AppSettings.BotToken);
                        await this.Client.SetStatusAsync(Discord.UserStatus.Online);
                    }
                    if (this.ClientReady)
                    {
                        await this.Process();
                    }
                }
                catch (Exception ex)
                {
                    this.WriteError(ex);
                }
                
                await Task.Delay(5000, stoppingToken);
            }
            
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            try
            {
                var content = arg.Content.Split(' ');
                var command = content.FirstOrDefault();
                if (command == BotCommand.CongTien && content.Length == 3)
                {
                    await this.CongTien(content, arg.Channel);
                }
            }
            catch (Exception ex)
            {
                this.WriteError(ex);
            }
            await Task.CompletedTask;

        }

        private async Task CongTien(string[] command, IMessageChannel channel)
        {
            try
            {
                var webmoney = Convert.ToInt32(command[2]);
                var username = command[1];
                var user = await this.AccountService.SingleBy(new { name = username });
                if(user != null)
                {
                    await this.UnitOfWork.CreateTransaction(async tran =>
                    {

                        var builder = new StringBuilder();
                        builder.AppendLine(string.Empty);
                        builder.AppendLine("-------------**CỘNG TIỀN**-------------");
                        builder.AppendLine($"**Tên tài khoản**: {username}");
                        builder.AppendLine($"**Số tiền ban đầu:** {user.WebMoney.ToString("#,##0")}");
                        builder.AppendLine($"**Số tiền cộng   :** {webmoney.ToString("#,##0")}");
                        builder.AppendLine($"**Số hiện tại    :** {(user.WebMoney + webmoney).ToString("#,##0")}");
                        builder.AppendLine($"**Thời gian  :**: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                        user.WebMoney += webmoney;
                        await this.AccountService.UpdateAsync(user, tran);
                        await this.BotMessageService.AddAsync(new BotMessageEntity
                        {
                            Message = builder.ToString().Base64Encode()
                        }, tran);
                        this.Logger.Info(builder.ToString());
                    });
                    await channel.SendMessageAsync("Cộng tiền thành công, vui lòng kiểm tra kênh report");
                }
                else
                {
                    await channel.SendMessageAsync("Tài khoản này không tồn tại");
                }
            }
            catch (FormatException)
            {
                await channel.SendMessageAsync("Cú pháp không hợp lệ. **/congtien username SoTien**");
            }
            catch (Exception ex)
            {
                await channel.SendMessageAsync(ex.Message);
                this.WriteError(ex);
            }
        }

        private void WriteError(Exception ex)
        {
            if(ex.InnerException != null)
            {
                WriteError(ex.InnerException);
            }
            this.Logger.Error(ex.Message);
            this.Logger.Error(ex.StackTrace);
        }
        private async Task Process()
        {
            var messages = await this.BotMessageService.FindBy(new { is_sent = false }).GetAsync<BotMessageEntity>();
            if (messages.Any())
            {
                this.Logger.Info("BOT SCAN: " + messages.Count());
            }
            foreach (var data in messages)
            {
                try
                {
                    var channelId = Convert.ToUInt64(data.Channel ?? ChannelConstant.REPORT.ToString());
                    var channel = this.Client.GetChannel(channelId) as IMessageChannel;
                    var text = data.Message.Base64Decode();
                    if (text != null && channel != null)
                    {
                        if (data.Image != null)
                        {
                            using (var r = new StreamReader(AppSettings.PathToImage + data.Image))
                            {
                                await channel.SendFileAsync(r.BaseStream, data.Image, data.Message.Base64Decode());
                            }
                        }
                        else
                        {
                            await channel.SendMessageAsync(text);
                        }
                    }
                    else
                    {
                        this.Logger.Error($"BOT_MESSAGE text: ${text?.ToString()} channelId: ${data.Channel ?? ChannelConstant.REPORT.ToString()}");
                    }
                    data.IsSent = true;
                    await this.BotMessageService.UpdateAsync(data);
                }
                catch (Exception ex)
                {
                    this.WriteError(ex);
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if(this.Client.ConnectionState == ConnectionState.Connected)
            {
                this.Client.Dispose();
            }
        }
    }
}
