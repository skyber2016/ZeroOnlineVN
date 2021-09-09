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
        IMapper Mapper { get; set; }
        IOptions<JwtSetting> JwtSetting { get; set; }
        ILoggerManager Logger { get; set; }
        Task CreateTransaction(Func<IDbTransaction, Task> action);
        IOptions<AppSettings> AppSettings { get; set; }
        DatabaseContext DatabaseContext { get; set; }
    }
    public class UnitOfWork : IUnitOfWork
    {
        [Dependency]
        public IMapper Mapper { get; set; }
        [Dependency]
        public IOptions<JwtSetting> JwtSetting { get; set; }
        [Dependency]
        public IOptions<AppSettings> AppSettings { get; set; }
        [Dependency]
        public ILoggerManager Logger { get; set; }

        public DatabaseContext DatabaseContext { get; set; }
        public UnitOfWork(DatabaseContext db)
        {
            DatabaseContext = db;
        }
        private static SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);
        public async Task CreateTransaction(Func<IDbTransaction, Task> action)
        {
            try
            {
                await SemaphoreSlim.WaitAsync();
                this.Logger.Info($"Begin transaction");
                try
                {
                    await action(null);
                    this.Logger.Info($"Transaction commit success");
                }
                catch (Exception ex)
                {
                    this.Logger.Error($"Transaction rollback: {ex.Message}");
                    throw;
                }
                finally
                {
                    this.Logger.Info($"End transaction");
                }
            }
            finally
            {
                SemaphoreSlim.Release();
            }

        }
    }
}
