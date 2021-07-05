using CORE_API.Cores;
using CORE_API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using System;
using System.Data;
using System.Threading.Tasks;
using Unity;

namespace CORE_API.Cores
{
    public interface IUnitOfWork 
    {
        ILoggerManager Logger { get; }
        Task CreateTransaction(Func<Task> action);
        DbContext DatabaseContext();
        IDbConnection GetDbConnection();
        IHostEnvironment GetEnvironment();
        IDbContextTransaction BeginTransaction();
        IDbContextTransaction GetCurrentTransaction();

    }
    public class UnitOfWork : IUnitOfWork
    {
        [Dependency]
        public ISystemCore SystemCore { get; set; }

        public ILoggerManager Logger
        {
            get
            {
                return this.SystemCore.GetLogger();
            }
        }
        public DbContext DatabaseContext()
        {
            return this.SystemCore.GetDbContext();

        }

        public IDbContextTransaction BeginTransaction()
        {
            return this.SystemCore.BeginTransaction();
        }

        public IDbConnection GetDbConnection()
        {
            return this.SystemCore.GetConnection();
        }

        public IDbContextTransaction GetCurrentTransaction()
        {
            return this.SystemCore.GetCurrentTransaction();
        }

        public async Task CreateTransaction(Func<Task> action)
        {
            this.Logger.Info($"Begin transaction");
            var dbContext = this.DatabaseContext();
            if(this.GetCurrentTransaction() == null)
            {
                using (IDbContextTransaction transaction = SystemCore.BeginTransaction())
                {
                    try
                    {
                        await action();
                        await dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        this.Logger.Info($"Transaction commit success");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error($"Transaction rollback: {ex.Message}");
                        await transaction.RollbackAsync();
                        throw;
                    }
                    finally
                    {
                        this.Logger.Info($"End transaction");
                    }
                }
            }
        }

        public IHostEnvironment GetEnvironment()
        {
            return this.SystemCore.GetEnvironment();
        }
    }
}
