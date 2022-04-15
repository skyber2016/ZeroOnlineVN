using AutoMapper;
using CORE_API.Cores;
using CORE_API.Databases;
using CORE_API.Helpers;
using Entity;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NEWS_MVC.Configurations;
using NEWS_MVC.Cores;
using NEWS_MVC.Database;
using NEWS_MVC.Helpers;
using NEWS_MVC.Services;
using NEWS_MVC.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Unity;
using WebMarkupMin.AspNetCore2;
using WebMarkupMin.NUglify;

namespace NEWS_MVC
{
    public class Startup
    {
        private IWebHostEnvironment CurrentEnvironment { get; set; }
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                 .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.AddWebMarkupMin(
            options =>
            {
                options.DisableMinification = false;
                options.DisableCompression = false;
                options.DisablePoweredByHttpHeaders = true;
                options.AllowMinificationInDevelopmentEnvironment = true;
                options.AllowCompressionInDevelopmentEnvironment = true;
            })
            .AddHtmlMinification(
                options =>
                {
                    options.CssMinifierFactory = new NUglifyCssMinifierFactory();
                    options.JsMinifierFactory = new NUglifyJsMinifierFactory();
                    options.MinificationSettings.RemoveRedundantAttributes = true;
                    options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                    options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
                })
            .AddHttpCompression();
            services.AddControllers();
            services.AddMvc(c =>
            {
                c.Filters.Add(new ResponseCacheAttribute() {
                    Duration = 120,
                    Location = ResponseCacheLocation.Any,
                    NoStore = false
                });
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
            })
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddControllersAsServices()
                .AddXmlSerializerFormatters()
            ;
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfig());
                mc.AddProfile(new AutoMapperEntityConfig());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            this.RegisterInterface(services);
            this.RegisterSetting(services);
            this.LoggerConfigure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseWebMarkupMin();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        #region Register Setting
        private void RegisterSetting(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<ConnectionSetting>(Configuration.GetSection("ConnectionStrings"));
        }
        #endregion
        public void ConfigureContainer(IUnityContainer container)
        {
            IQueryableExtension.Configure(container);
        }
        #region register interface
        private void RegisterInterface(IServiceCollection services)
        {

            services.AddScoped<ActionDescriptor>();
            services.AddScoped<ILoggerManager, LoggerHelper>();
            services.AddScoped(typeof(IGeneralService<>), typeof(GeneralService<>));
            services.AddScoped<ISystemCore, SystemCore>();
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DatabaseCoreContext>();
            services.AddScoped<DatabaseContext>();
            var ass = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.ImplementedInterfaces.Any() && x.Name.EndsWith("Service")).ToList();
            foreach (var imp in ass)
            {
                foreach (var inter in imp.ImplementedInterfaces)
                {
                    var interf = inter.Name.Remove(0, 1);
                    if (imp.Name.Equals(interf))
                    {
                        services.AddScoped(inter, imp);
                    }
                }
            }
        }
        #endregion

        #region config logger

        private void LoggerConfigure()
        {
            XmlDocument log4netConfig = new XmlDocument();
            using (var fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"))
            {
                log4netConfig.Load(fs);

                var repo = LogManager.CreateRepository(
                        Assembly.GetEntryAssembly(),
                        typeof(log4net.Repository.Hierarchy.Hierarchy));

                XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
            }
        }
        #endregion
    }
}
