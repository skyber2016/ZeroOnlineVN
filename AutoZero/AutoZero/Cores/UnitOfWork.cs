using AutoZero.Configurations;
using AutoZero.Helpers;
using AutoZero.MVVM.ViewModel;
using System;
using System.Configuration;
namespace AutoZero.Cores
{
    public interface IUnitOfWork 
    {
        ILoggerManager Logger { get; set; }
        AppSettings AppSettings { get; set; }
        MainViewModel ViewModel { get; set; }
    }
    public class UnitOfWork : IUnitOfWork
    {
        public AppSettings AppSettings { get; set; } = new AppSettings()
        {
            IpMid = ConfigurationManager.AppSettings["IpMid"],
            IpServer = ConfigurationManager.AppSettings["IpServer"],
            PortGameMid = Convert.ToInt32(ConfigurationManager.AppSettings["PortGameMid"]),
            PortGameServer = Convert.ToInt32(ConfigurationManager.AppSettings["PortGameServer"]),
            PortLoginMid = Convert.ToInt32(ConfigurationManager.AppSettings["PortLoginMid"]),
            PortLoginServer = Convert.ToInt32(ConfigurationManager.AppSettings["PortLoginServer"])
        };
        public ILoggerManager Logger { get; set; } = new LoggerHelper();
        public MainViewModel ViewModel { get; set; }
        public UnitOfWork(MainViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }
    }
}
