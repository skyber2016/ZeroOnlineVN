using API.Common;
using API.Configurations;
using API.Entities;
using API.Helpers;
using API.Services.Interfaces;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NCrontab;
using SqlKata.Execution;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace API.Cores
{
    public class BotDiscordHostedService :  BackgroundService 
    {
        [Dependency]
        public IGeneralService<BotMessageEntity> BotMessageService { get; set; }
        [Dependency]
        public IOptions<AppSettings> AppSettings { get; set; }
        [Dependency]
        public ILoggerManager Logger { get; set; }
        [Dependency]
        public IGeneralService<AccountEntity> AccountService { get; set; }
        [Dependency]
        public IUnitOfWork UnitOfWork { get; set; }
        private readonly CrontabSchedule _schedule;
        private DateTime _nextRun { get; set; }

        private string Schedule => "*/10 * * * * *"; //Runs every 10 seconds
        private DiscordSocketClient Client { get; set; }
        private System.Timers.Timer Timer { get; set; }
        public BotDiscordHostedService()
        {
            this.Timer = new System.Timers.Timer();
            this.Timer.Interval = 5000;
            this.Timer.Elapsed += Timer_Elapsed;
            this.Client = new DiscordSocketClient();
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Process().Wait();
            }
            catch (Exception ex)
            {
                this.WriteError(ex);
            }
        }

        private bool ClientReady { get; set; }
        private async Task Client_Ready()
        {
            this.ClientReady = true;
            this.Timer.Start();
            await Task.CompletedTask;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.Client.Ready += async () => await this.Client_Ready();
            this.Client.MessageReceived += Client_MessageReceived;
            await this.Client.StartAsync();
            await this.Client.LoginAsync(Discord.TokenType.Bot, this.AppSettings.Value.BotToken);
            await this.Client.SetStatusAsync(Discord.UserStatus.Online);
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            var content = arg.Content.Split(' ');
            var command = content.FirstOrDefault();
            if(command == BotCommand.CongTien && content.Length == 3)
            {
                await this.CongTien(content, arg.Channel);
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
            }
        }

        private void WriteError(Exception ex)
        {
            if(ex.InnerException != null)
            {
                WriteError(ex.InnerException);
            }
            this.Logger.Error(ex.Message);
        }
        private async Task Process()
        {
            var messages = await this.BotMessageService.FindBy(new { is_sent = false }).GetAsync<BotMessageEntity>();
            foreach (var data in messages)
            {
                foreach (var ch in this.AppSettings.Value.Channels)
                {
                    try
                    {
                        var channel = this.Client.GetChannel(ch) as IMessageChannel;
                        var text = data.Message.Base64Decode();
                        if (text != null && channel != null)
                        {
                            await channel.SendMessageAsync(text);
                        }
                        else
                        {
                            this.Logger.Error($"BOT_MESSAGE text: ${text?.ToString()} channelId: ${ch}");
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
