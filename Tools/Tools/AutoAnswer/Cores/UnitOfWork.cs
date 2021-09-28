using API.Configurations;
using API.Helpers;
using AutoAnswer.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Threading.Tasks;

namespace API.Cores
{
    public interface IUnitOfWork 
    {
        ILoggerManager Logger { get; set; }
        Task CreateTransaction(Func<IDbTransaction, Task> action);
        IOptions<AppSettings> AppSettings { get; set; }
        T GetInstance<T>();
        IMemoryCache Cache { get; set; }
        IAnswerService AnswerService { get; set; }
    }
    public class UnitOfWork : IUnitOfWork
    {
        public IOptions<AppSettings> AppSettings { get; set; }
        public ILoggerManager Logger { get; set; }
        public IServiceProvider ServicesProvider { get; set; }
        public IMemoryCache Cache { get; set; }
        public IAnswerService AnswerService { get; set; }


        public UnitOfWork(IOptions<AppSettings> AppSettings, ILoggerManager Logger, IServiceProvider services, IMemoryCache caching, IAnswerService answerService)
        {
            this.AnswerService = answerService;
            this.Cache = caching;
            this.ServicesProvider = services;
            this.Logger = Logger;
            this.AppSettings = AppSettings;
        }

        public T GetInstance<T>()
        {
            return this.ServicesProvider.GetService<T>();
        }
        public async Task CreateTransaction(Func<IDbTransaction,Task> action)
        {
            await action(null);
        }
    }
}
