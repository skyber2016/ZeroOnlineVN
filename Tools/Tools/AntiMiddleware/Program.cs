using API.Configurations;
using API.Cores;
using API.Database;
using API.Helpers;
using API.Services;
using API.Services.Interfaces;
using Discord.WebSocket;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

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
                    services.Configure<ConnectionSetting>(hostContext.Configuration.GetSection("ConnectionStrings"));
                    services.AddSingleton<DatabaseContext>();
                    services.AddSingleton<DiscordSocketClient>();
                    services.AddSingleton<ILoggerManager, LoggerHelper>();
                    services.AddSingleton(typeof(IGeneralService<>), typeof(GeneralService<>));
                    services.AddSingleton<IUnitOfWork, UnitOfWork>();
                });
    }
}
