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
