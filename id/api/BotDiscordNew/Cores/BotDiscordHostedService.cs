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
        public IGeneralService<GiftCodeEntity> GiftCodeService { get; set; }

        public BotDiscordHostedService(
            IOptions<AppSettings> options,
            IUnitOfWork unitOfWork,
            ILoggerManager logger,
            IGeneralService<BotMessageEntity> botMessageService,
            IGeneralService<AccountEntity> accountService,
            IGeneralService<GiftCodeEntity> giftCodeService
            )
        {
            this.AppSettings = options.Value;
            this.UnitOfWork = unitOfWork;
            this.Logger = logger;
            this.BotMessageService = botMessageService;
            this.AccountService = accountService;
            this.GiftCodeService = giftCodeService;
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
                        this.Logger.Info("BOT BEGIN LOGIN");
                        await this.Client.StartAsync();
                        await this.Client.LoginAsync(Discord.TokenType.Bot, this.AppSettings.BotToken);
                        await this.Client.SetStatusAsync(Discord.UserStatus.Online);
                        this.Logger.Info("BOT LOGIN SUCCESS");
                        await Task.Delay(5000, stoppingToken);
                        continue;
                    }
                    else {
                        this.Logger.Info($"BOT STATE {this.Client.ConnectionState}");
                    }

                    if (this.Client.ConnectionState == ConnectionState.Disconnected)
                    {
                        this.ClientReady = false;
                        this.Client.Dispose();
                        this.Client = null;
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
                else if (command == BotCommand.GiftCode && content.Length == 4)
                {
                    await this.GiftCode(content, arg.Channel);
                }
            }
            catch (Exception ex)
            {
                this.WriteError(ex);
            }
            await Task.CompletedTask;

        }

        private async Task GiftCode(string[] command, IMessageChannel channel)
        {
            try
            {
                var code = command[1];
                var itemId = command[2];
                var type = command[3];
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(type))
                {
                    throw new FormatException();
                }
                var typeCode = Convert.ToInt32(type);
                if(typeCode != GiftCodeTypeConstant.SINGLE && typeCode != GiftCodeTypeConstant.MULTIPLE)
                {
                    throw new FormatException();
                }
                var giftCode = new GiftCodeEntity
                {
                    Code = code,
                    ItemId = Convert.ToInt32(itemId),
                    Type = typeCode
                };
                await this.GiftCodeService.AddAsync(giftCode);
                var build = new StringBuilder();
                build.AppendLine($"-------- **Tạo Gift Code** --------");
                build.AppendLine($"**Gift Code**: {code}");
                var loaiCode = typeCode == GiftCodeTypeConstant.SINGLE ? "1 cho 1" : "1 cho nhiều";
                build.AppendLine($"**Loại**: { loaiCode }");
                build.AppendLine($"**Action ID**: { giftCode.ItemId }");
                await channel.SendMessageAsync(build.ToString());
            }
            catch (FormatException)
            {
                await channel.SendMessageAsync("Cú pháp không hợp lệ. **/code {mã} {action_id} {loại} '1': 1 cho 1 - '2': 1 cho nhiều tài khoản**");
            }
            
        }

        private async Task CongTien(string[] command, IMessageChannel channel)
        {
            try
            {
                var webmoney = Convert.ToInt32(command[2]);
                var username = command[1];
                var account = await this.AccountService.SingleBy(new { name = username });
                if(account != null)
                {
                    await this.UnitOfWork.CreateTransaction(async tran =>
                    {

                        var builder = new StringBuilder();
                        builder.AppendLine(string.Empty);
                        builder.AppendLine("-------------**CỘNG TIỀN**-------------");
                        builder.AppendLine($"**Tên tài khoản**: {username}");
                        builder.AppendLine($"**Số tiền ban đầu:** {account.WebMoney.ToString("#,##0")}");
                        builder.AppendLine($"**Số tiền cộng   :** {webmoney.ToString("#,##0")}");
                        builder.AppendLine($"**Số hiện tại    :** {(account.WebMoney + webmoney).ToString("#,##0")}");
                        builder.AppendLine($"**Thời gian  :**: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                        account.WebMoney += webmoney;
                        account.CheckSum = account.GetCheckSum();
                        account.Wheel += webmoney / 50000;
                        account.WebMoneyUsing += webmoney;
                        var upgradeVIP = false;
                        if (account.WebMoneyUsing >= VipConstant.VIP_6 && account.VIP < 6)
                        {
                            account.VIP = 6;
                            upgradeVIP = true;
                        }
                        else if (account.WebMoneyUsing >= VipConstant.VIP_5 && account.VIP < 5)
                        {
                            account.VIP = 5;
                            upgradeVIP = true;
                        }
                        else if (account.WebMoneyUsing >= VipConstant.VIP_4 && account.VIP < 4)
                        {
                            account.VIP = 4;
                            upgradeVIP = true;
                        }
                        else if (account.WebMoneyUsing >= VipConstant.VIP_3 && account.VIP < 3)
                        {
                            account.VIP = 3;
                            upgradeVIP = true;
                        }
                        else if (account.WebMoneyUsing >= VipConstant.VIP_2 && account.VIP < 2)
                        {
                            account.VIP = 2;
                            upgradeVIP = true;
                        }
                        else if (account.WebMoneyUsing >= VipConstant.VIP_1 && account.VIP < 1)
                        {
                            account.VIP = 1;
                            upgradeVIP = true;
                        }
                        builder.AppendLine($"**Vòng quay được cộng:**: {webmoney / 50000}");
                        builder.AppendLine($"**Tổng vòng quay:**: {account.Wheel}");
                        if (upgradeVIP)
                        {
                            builder.AppendLine($"**Đạt mốc VIP mới:** {account.VIP}");
                        }
                        await this.AccountService.UpdateAsync(account, tran);
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
                        //if (data.Image != null)
                        //{
                        //    using (var r = new StreamReader(AppSettings.PathToImage + data.Image))
                        //    {
                        //        await channel.SendFileAsync(r.BaseStream, data.Image, data.Message.Base64Decode());
                        //    }
                        //}
                        //else
                        //{
                        //    await channel.SendMessageAsync(text);
                        //}
                        await channel.SendMessageAsync(text);
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
