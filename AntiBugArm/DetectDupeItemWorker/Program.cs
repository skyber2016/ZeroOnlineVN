using DetectDupeItemCore.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DetectDupeItemCore
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .UseSystemd()
                 .ConfigureAppConfiguration((hostContext, config) =>
                 {
                     config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                     config.AddEnvironmentVariables();
                 })
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddHostedService<Worker>();
                     services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
                 });


        

    }
}
