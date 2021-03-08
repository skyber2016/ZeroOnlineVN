using AutoMapper;
using Forum_API.Configurations;
using Forum_API.Database;
using Forum_API.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Cores
{
    public interface IUnitOfWork 
    {
        IMapper Mapper { get; set; }
        IOptions<JwtSetting> JwtSetting { get; set; }
        ILoggerManager Logger { get; set; }
        Task CreateTransaction(Func<DatabaseContext, Task> action);
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
        public async Task CreateTransaction(Func<DatabaseContext, Task> action)
        {
            this.Logger.Info($"Begin transaction");
            using (var transaction = await this.DatabaseContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await action(this.DatabaseContext);
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
}
