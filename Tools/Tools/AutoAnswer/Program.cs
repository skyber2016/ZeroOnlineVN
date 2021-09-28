using API.Configurations;
using API.Cores;
using API.Helpers;
using AutoAnswer.Services;
using AutoAnswer.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoAnswer
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
                    services.AddSingleton<ILoggerManager, LoggerHelper>();
                    services.AddSingleton<IAnswerService, AnswerService>();
                    services.AddSingleton<IUnitOfWork, UnitOfWork>();
                    services.AddSingleton<IMemoryCache, MemoryCache>();
                });
    }
}
