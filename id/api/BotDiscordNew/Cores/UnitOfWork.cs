using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Threading.Tasks;
using Unity;
using API.Configurations;
using API.Database;
using API.Helpers;
using System.Threading;

namespace API.Cores
{
    public interface IUnitOfWork 
    {
        ILoggerManager Logger { get; set; }
        Task CreateTransaction(Func<IDbTransaction, Task> action);
        IOptions<AppSettings> AppSettings { get; set; }
    }
    public class UnitOfWork : IUnitOfWork
    {
        public IOptions<AppSettings> AppSettings { get; set; }
        public ILoggerManager Logger { get; set; }

        public UnitOfWork(IOptions<AppSettings> AppSettings, ILoggerManager Logger)
        {
            this.Logger = Logger;
            this.AppSettings = AppSettings;
        }

        private static SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);
        public async Task CreateTransaction(Func<IDbTransaction,Task> action)
        {
            try
            {
                await SemaphoreSlim.WaitAsync();
                await action(null);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
    }
}
