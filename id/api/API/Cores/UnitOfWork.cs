using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Threading.Tasks;
using Unity;
using API.Configurations;
using API.Database;
using API.Helpers;

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
        public async Task CreateTransaction(Func<IDbTransaction,Task> action)
        {
            try
            {
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
            }
            
        }
    }
}
