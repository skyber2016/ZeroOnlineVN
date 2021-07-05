using CORE_API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace CORE_API.Cores
{
    public interface ISystemCore
    {
        IDbContextTransaction BeginTransaction();
        ILoggerManager GetLogger();
        DbContext GetDbContext();
        IDbConnection GetConnection();
        IHostEnvironment GetEnvironment();
        IDbContextTransaction GetCurrentTransaction();
    }
}
