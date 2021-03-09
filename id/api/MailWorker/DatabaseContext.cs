using Cambopay_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace MailWorker
{
    public class DatabaseContext : DbContext
    {
        private static string ConnectionString { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<MailSendingEntity> MailSending { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (ConnectionString == null)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                ConnectionString = configuration.GetConnectionString("DefaultConnection");
            }
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseNpgsql(ConnectionString);
        }
    }
}
