using GameServer.Core.Utils;
using System.Diagnostics;
using System.Reflection;

namespace GameServer
{
    public static class Program
    {

        public static Logging Logging;
        public static readonly Stopwatch Watch = Stopwatch.StartNew();
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                {
                    Console.WriteLine("Not found args");
                    Thread.Sleep(10000);
                    return;
                }
                Logging = new Logging();
                Logging.Setup(Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location));
                Logging.LogAllExceptions();
                var username = args[0];
                var port = args[1];
                
                Logging.Setup(Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location) + "_user" + username);
                GameServer.Core.Server.Starting(Convert.ToInt32(port), username);
                var timeout = TimeSpan.FromMinutes(1);
                var task = Task.Run(async () =>
                {
                    while (true)
                    {
                        if (Watch.IsRunning && Watch.ElapsedMilliseconds > timeout.TotalMilliseconds)
                        {
                            break;
                        }
                        await Task.Delay(1000);
                    }
                });
                Task.WaitAll(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
                Console.WriteLine(ex.GetBaseException().StackTrace);
                Console.Read();
            }
        }
    }
}
