using AutoMapper;
using CORE_API;
using CORE_API.Configurations;
using CORE_API.Cores;
using CORE_API.Databases;
using CORE_API.Helpers;
using CORE_API.Middlewares;
using NEWS_API.Configurations;
using NEWS_API.Cores;
using NEWS_API.Database;
using NEWS_API.DTO.Error.Responses;
using NEWS_API.Helpers;
using NEWS_API.Services;
using NEWS_API.Services.Interfaces;
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
using NEWS_API.Services;
using Prometheus;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Unity;

namespace NEWS_API
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
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.RegisterSetting(services);

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddControllers();
            services.AddMvc(c =>
            {
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
            })
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddControllersAsServices()
                .AddXmlSerializerFormatters()
            ;
            #region enable sync
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            #endregion
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfig());
                mc.AddProfile(new AutoMapperEntityConfig());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton<PrometheusReporter>();
            // Register interface
            this.RegisterInterface(services);
            this.LoggerConfigure();
        }
        #region App
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("MyPolicy");
            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
        #endregion
        #region Configuration
        public void ConfigureContainer(IUnityContainer container)
        {
            IQueryableExtension.Configure(container);
        }
        #region Register Setting
        private void RegisterSetting(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<ConnectionSetting>(Configuration.GetSection("ConnectionStrings"));
        }
        #endregion
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
        #endregion
    }
}
