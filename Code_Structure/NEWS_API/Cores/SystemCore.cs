using CORE_API.Cores;
using CORE_API.Helpers;
using NEWS_API.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using System.Data;
using Unity;

namespace NEWS_API.Cores
{
    public class SystemCore : ISystemCore
    {
        [Dependency]
        public DatabaseContext DatabaseContext { get; set; }
        [Dependency]
        public ILoggerManager LoggerManager { get; set; }
        [Dependency]
        public IWebHostEnvironment Environment { get; set; }

        public IDbContextTransaction BeginTransaction()
        {
            return this.DatabaseContext.Database.BeginTransaction();
        }

        public IDbConnection GetConnection()
        {
            return this.DatabaseContext.Database.GetDbConnection();
        }

        public IDbContextTransaction GetCurrentTransaction()
        {
            return this.DatabaseContext.Database.CurrentTransaction;
        }

        public DbContext GetDbContext()
        {
            return this.DatabaseContext;
        }


        public IHostEnvironment GetEnvironment()
        {
            return Environment;
        }

        public ILoggerManager GetLogger()
        {
            return this.LoggerManager;
        }
    }
}
