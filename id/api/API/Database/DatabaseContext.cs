using API.Helpers;
using Microsoft.Extensions.Configuration;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.Odbc;

namespace API.Database
{
    public class DatabaseContext
    {
        public Guid SessionId { get; set; }
        public QueryFactory Factory { get; set; }
        private static string ConnectionString { get; set; }
        public DatabaseContext(ILoggerManager logger)
        {
            this.SessionId = Guid.NewGuid();
            if (ConnectionString == null)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                ConnectionString = configuration.GetConnectionString("DefaultConnection");
            }
            this.Factory = new QueryFactory(new OdbcConnection(ConnectionString), new MySqlCompiler());
            this.Factory.Logger = result => logger.Info(result.ToString());
        }
    }
}
