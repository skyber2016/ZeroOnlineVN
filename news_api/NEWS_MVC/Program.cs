using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Unity.Microsoft.DependencyInjection;

namespace NEWS_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseUnityServiceProvider()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseIIS();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
