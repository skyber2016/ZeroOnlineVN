using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using System.Reflection;
using System.Xml;
using ServerForward;
public static class Program
{
    private static ProxyServer ProxyServer { get; set; }
    public static async Task<int> Main(string[] args)
    {
        
        XmlDocument xmlDocument = new XmlDocument();
        using (FileStream inStream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"))
        {
            xmlDocument.Load(inStream);
            XmlConfigurator.Configure(LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(Hierarchy)), xmlDocument["log4net"]);
        }
        ProxyServer = new ProxyServer();
        ProxyServer.Start();
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            Console.WriteLine("Cancel event triggered");
            ProxyServer.Stop();
            eventArgs.Cancel = true;
        };
        while (ProxyServer.IsListening)
        {
            await Task.Delay(1000);
        }
        return 0;
    }
}