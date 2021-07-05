using CORE_API.Databases;
using NEWS_API.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Unity;

namespace NEWS_API.Database
{
    public class DatabaseContext : DatabaseCoreContext
    {
        [Dependency]
        public IOptions<ConnectionSetting> ConnectionSetting { get; set; }
        [Dependency]
        public IWebHostEnvironment Env { get; set; }
        public readonly ILoggerFactory MyLoggerFactory;

        
        private static string ConnectionString { get; set; }

        public DatabaseContext()
        {
            this.MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddDebug(); });
        }

        public string GetConnectionString()
        {
            if (ConnectionString == null)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                return configuration.GetConnectionString("DefaultConnection");
            }
            return ConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConnectionString = this.GetConnectionString();
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseLazyLoadingProxies()
                .UseMySQL(ConnectionString)
                ;
            if (Env.IsDevelopment())
            {
                optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
