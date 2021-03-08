using Forum_API.Configurations;
using Forum_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Unity;

namespace Forum_API.Database
{
    public class DatabaseContext : DbContext
    {
        [Dependency]
        public IOptions<ConnectionSetting> ConnectionSetting { get; set; }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AreaEntity> Areas { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<FunctionEntity> Functions { get; set; }
        public DbSet<MailQueueEntity> MailQueues { get; set; }
        public DbSet<MenuEntity> Menus { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<ShoutBoxEntity> ShoutBoxs { get; set; }
        public DbSet<SystemConfigEntity> SystemConfigs { get; set; }
        public DbSet<SystemLogEntity> SystemLogs { get; set; }
        public DbSet<SystemMessageEntity> SystemMessages { get; set; }
        public DbSet<TopicEntity> Topics { get; set; }
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<RoleFunctionEntity> RoleFunctions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var setting = ConnectionSetting.Value;
            optionsBuilder
                .UseNpgsql(setting.ConnectionString)
                .UseLazyLoadingProxies()
                ;
        }
    }
}
