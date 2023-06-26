using API.Configurations;
using API.Cores;
using API.Database;
using API.Helpers;
using Discord.WebSocket;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace BotDiscord
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bytes = new byte[] { 24, 0, 91, 4, 237, 14, 76, 0, 4, 0, 0, 0, 60, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };
            var hex = BitConverter.ToString(bytes);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Middleware>();
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
                    services.AddSingleton<DatabaseContext>();
                    services.AddSingleton<DiscordSocketClient>();
                    services.AddSingleton<ILoggerManager, LoggerHelper>();
                    services.AddSingleton<IUnitOfWork, UnitOfWork>();
                    services.AddSingleton<IMemoryCache, MemoryCache>();
                });
    }
}
