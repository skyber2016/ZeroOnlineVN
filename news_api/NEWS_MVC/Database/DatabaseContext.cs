using CORE_API.Databases;
using NEWS_MVC.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Unity;
using Entity;

namespace NEWS_MVC.Database
{
    public class DatabaseContext : DatabaseCoreContext
    {
        [Dependency]
        public IOptions<ConnectionSetting> ConnectionSetting { get; set; }
        [Dependency]
        public IWebHostEnvironment Env { get; set; }
        public readonly ILoggerFactory MyLoggerFactory;

        public DbSet<ActionschedulerActionsEntity> ActionschedulerActions { get; set; }
        public DbSet<ActionschedulerClaimsEntity> ActionschedulerClaims { get; set; }
        public DbSet<ActionschedulerGroupsEntity> ActionschedulerGroups { get; set; }
        public DbSet<ActionschedulerLogsEntity> ActionschedulerLogs { get; set; }
        public DbSet<CommentmetaEntity> Commentmeta { get; set; }
        public DbSet<CommentsEntity> Comments { get; set; }
        public DbSet<LinksEntity> Links { get; set; }
        public DbSet<OptionsEntity> Options { get; set; }
        public DbSet<PostmetaEntity> Postmeta { get; set; }
        public DbSet<PostsEntity> Posts { get; set; }
        public DbSet<TermmetaEntity> Termmeta { get; set; }
        public DbSet<TermsEntity> Terms { get; set; }
        public DbSet<TermRelationshipsEntity> TermRelationships { get; set; }
        public DbSet<TermTaxonomyEntity> TermTaxonomy { get; set; }
        public DbSet<UsermetaEntity> Usermeta { get; set; }
        public DbSet<UsersEntity> Users { get; set; }
        public DbSet<WcAdminNotesEntity> WcAdminNotes { get; set; }
        public DbSet<WcAdminNoteActionsEntity> WcAdminNoteActions { get; set; }
        public DbSet<WcCategoryLookupEntity> WcCategoryLookup { get; set; }
        public DbSet<WcCustomerLookupEntity> WcCustomerLookup { get; set; }
        public DbSet<WcDownloadLogEntity> WcDownloadLog { get; set; }
        public DbSet<WcOrderCouponLookupEntity> WcOrderCouponLookup { get; set; }
        public DbSet<WcOrderProductLookupEntity> WcOrderProductLookup { get; set; }
        public DbSet<WcOrderStatsEntity> WcOrderStats { get; set; }
        public DbSet<WcOrderTaxLookupEntity> WcOrderTaxLookup { get; set; }
        public DbSet<WcProductMetaLookupEntity> WcProductMetaLookup { get; set; }
        public DbSet<WcReservedStockEntity> WcReservedStock { get; set; }
        public DbSet<WcTaxRateClassesEntity> WcTaxRateClasses { get; set; }
        public DbSet<WcWebhooksEntity> WcWebhooks { get; set; }
        public DbSet<WoocommerceApiKeysEntity> WoocommerceApiKeys { get; set; }
        public DbSet<WoocommerceAttributeTaxonomiesEntity> WoocommerceAttributeTaxonomies { get; set; }
        public DbSet<WoocommerceDownloadableProductPermissionsEntity> WoocommerceDownloadableProductPermissions { get; set; }
        public DbSet<WoocommerceLogEntity> WoocommerceLog { get; set; }
        public DbSet<WoocommerceOrderItemmetaEntity> WoocommerceOrderItemmeta { get; set; }
        public DbSet<WoocommerceOrderItemsEntity> WoocommerceOrderItems { get; set; }
        public DbSet<WoocommercePaymentTokenmetaEntity> WoocommercePaymentTokenmeta { get; set; }
        public DbSet<WoocommercePaymentTokensEntity> WoocommercePaymentTokens { get; set; }
        public DbSet<WoocommerceSessionsEntity> WoocommerceSessions { get; set; }
        public DbSet<WoocommerceShippingZonesEntity> WoocommerceShippingZones { get; set; }
        public DbSet<WoocommerceShippingZoneLocationsEntity> WoocommerceShippingZoneLocations { get; set; }
        public DbSet<WoocommerceShippingZoneMethodsEntity> WoocommerceShippingZoneMethods { get; set; }
        public DbSet<WoocommerceTaxRatesEntity> WoocommerceTaxRates { get; set; }
        public DbSet<WoocommerceTaxRateLocationsEntity> WoocommerceTaxRateLocations { get; set; }
        public DbSet<WrcCachesEntity> WrcCaches { get; set; }
        public DbSet<WrcRelationsEntity> WrcRelations { get; set; }


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
