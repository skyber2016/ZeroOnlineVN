using API.Common;
using API.Configurations;
using API.Entities;
using API.Helpers;
using API.Services.Interfaces;
using BotDiscord;
using BotDiscordNew;
using Discord;
using Discord.WebSocket;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        public ConnectionSetting ConnectionSetting { get; set; }
        public TrackingLogSetting TrackingLogSetting { get; set; }
        public ILoggerManager Logger { get; set; }
        public IGeneralService<AccountEntity> AccountService { get; set; }
        public IGeneralService<UserEntity> UserService { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        private DiscordSocketClient Client { get; set; }
        public IGeneralService<GiftCodeEntity> GiftCodeService { get; set; }

        public BotDiscordHostedService(
            IOptions<AppSettings> options,
            IUnitOfWork unitOfWork,
            ILoggerManager logger,
            IGeneralService<BotMessageEntity> botMessageService,
            IGeneralService<AccountEntity> accountService,
            IGeneralService<GiftCodeEntity> giftCodeService,
            IGeneralService<UserEntity> userService,
            IOptions<TrackingLogSetting> trackingLogSetting,
            IOptions<ConnectionSetting> connectionSettting
            )
        {
            this.AppSettings = options.Value;
            this.UnitOfWork = unitOfWork;
            this.Logger = logger;
            this.BotMessageService = botMessageService;
            this.AccountService = accountService;
            this.GiftCodeService = giftCodeService;
            this.UserService = userService;
            this.TrackingLogSetting = trackingLogSetting.Value;
            this.ConnectionSetting = connectionSettting.Value;
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
                    await this.TrackingLog();
                }
                catch (Exception ex)
                {
                    this.WriteError(ex);
                }
                
                await Task.Delay(5000, stoppingToken);
            }
            
        }

        private async Task TrackingLog()
        {
            using var http = new HttpClient();
            var host = ConnectionSetting.LogAPI;
            var fileName = $"emoney_cost {DateTime.Now.ToString("yyyy-M-d")}.log";
            var response = await http.GetAsync($"{host}/{fileName}?time={DateTime.Now.Ticks}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var lastLines = GetLastLineNumber(fileName);
                var allLines = content.Split('\n').Select(x=>x.Trim()).Where(x=>!string.IsNullOrEmpty(x));
                var lines = allLines.Skip(lastLines).ToList();
                var totalLines = allLines.Count();
                if (totalLines > lastLines)
                {
                    foreach (var line in lines)
                    {
                        try
                        {
                            await Tracking(line);
                            lastLines += 1;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.GetBaseException().Message);
                        }
                        finally
                        {
                            var pathToFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);
                            using (var w = new StreamWriter(pathToFile, false))
                            {
                                w.Write(lastLines.ToString());
                            }
                        }
                    }
                }
            }
        }

        private async Task Tracking(string plainText)
        {
            var formats = plainText.Split(',');
            if (formats.Length == 12)
            {
                var userId = formats[1];
                var price = formats[2];
                var itemId = formats[4];
                var emoney = formats[10];
                var lastStr = formats[11];
                var itemTracking = this.TrackingLogSetting.Items.FirstOrDefault(x => x.ItemId == itemId);
                if (itemTracking != null)
                {
                    var user = await this.UserService.SingleBy(new
                    {
                        id = userId,
                    });
                    string dateString = lastStr.Split("--").LastOrDefault();
                    string format = "ddd MMM dd HH:mm:ss yyyy";
                    var msg = $"";
                    if (DateTime.TryParseExact(dateString?.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                    {
                        msg = $"[{result.ToString("dd/MM/yyyy HH:mm:ss")}]";
                    }
                    else
                    {
                        msg = $"[{dateString}]";
                    }
                    msg = $"{msg} {user?.Name ?? userId} vừa mua {itemId} ({itemTracking.Name}) giá: {price}, zps còn lại: {Convert.ToInt32(emoney).ToString("#,##0")}";
                    await this.BotMessageService.AddAsync(new BotMessageEntity
                    {
                        Channel = ChannelConstant.ATOM.ToString(),
                        Message = msg.Base64Encode(),
                        CreatedDate = DateTime.Now,
                        IsSent = false
                    });
                }
            }
        }

        private int GetLastLineNumber(string fileName)
        {
            try
            {
                var pathToFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);
                if (File.Exists(pathToFile))
                {
                    var text = File.ReadAllText(pathToFile);
                    return Convert.ToInt32(text);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
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
                        account.Wheel += webmoney / AppSettings.MoneyToWheel;
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
                        builder.AppendLine($"**Vòng quay được cộng:**: {webmoney / AppSettings.MoneyToWheel}");
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
