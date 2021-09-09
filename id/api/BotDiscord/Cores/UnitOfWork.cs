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
        DatabaseContext DatabaseContext { get; set; }
    }
    public class UnitOfWork : IUnitOfWork
    {
        public IOptions<AppSettings> AppSettings { get; set; }
        public ILoggerManager Logger { get; set; }

        public DatabaseContext DatabaseContext { get; set; }
        public UnitOfWork(DatabaseContext db, IOptions<AppSettings> AppSettings, ILoggerManager Logger)
        {
            DatabaseContext = db;
            this.Logger = Logger;
            this.AppSettings = AppSettings;
        }

        private static SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);
        public async Task CreateTransaction(Func<IDbTransaction,Task> action)
        {
            try
            {
                await SemaphoreSlim.WaitAsync();
                this.Logger.Info($"Begin transaction");
                this.DatabaseContext.Factory.Connection.Open();
                using (IDbTransaction transaction = this.DatabaseContext.Factory.Connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        await action(transaction);
                        transaction.Commit();
                        this.Logger.Info($"Transaction commit success");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error($"Transaction rollback: {ex.Message}");
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        this.Logger.Info($"End transaction");
                    }
                }
            }
            finally
            {
                if(this.DatabaseContext.Factory.Connection.State == ConnectionState.Open)
                {
                    this.DatabaseContext.Factory.Connection.Close();
                }
                SemaphoreSlim.Release();
            }
            
        }
    }
}
