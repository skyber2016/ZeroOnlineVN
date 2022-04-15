using API.Configurations;
using API.Cores;
using API.Database;
using API.Helpers;
using API.Services;
using API.Services.Interfaces;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotDiscordNew
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddHostedService<Worker>();
                    services.AddHostedService<BotDiscordHostedService>();
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
