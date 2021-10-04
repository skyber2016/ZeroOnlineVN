using API.Configurations;
using API.Helpers;
using AutoAnswer.Services;
using AutoAnswer.Services.Interfaces;
using System;
using System.Configuration;

namespace API.Cores
{
    public interface IUnitOfWork 
    {
        ILoggerManager Logger { get; set; }
        AppSettings AppSettings { get; set; }
        IAnswerService AnswerService { get; set; }
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
        public IAnswerService AnswerService { get; set; } = new AnswerService();

    }
}
