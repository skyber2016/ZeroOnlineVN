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
        public QueryFactory Factory { get; set; }
        private static string DbAPI { get; set; }
        public DatabaseContext(ILoggerManager logger)
        {
            if (DbAPI == null)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                DbAPI = configuration.GetConnectionString("DbAPI");
            }
            this.Factory = new QueryFactory(new OdbcConnection(), new MySqlCompiler(), DbAPI);
            this.Factory.Logger = result =>
            {
                //logger.Info(result.ToString());
            };
            QueryFactory.HttpLoggerError = message =>
            {
                logger.Error(message);
            };
            QueryFactory.HttpLoggerInfo = message =>
            {
                logger.Info(message);
            };
        }
    }
}
