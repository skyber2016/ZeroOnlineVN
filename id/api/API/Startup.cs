using API.Configurations;
using API.Cores;
using API.Database;
using API.DTO.Error.Responses;
using API.Helpers;
using API.Middlewares;
using API.Security;
using API.Services;
using API.Services.Authenticate;
using API.Services.Interfaces;
using AutoMapper;
using Discord.WebSocket;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prometheus;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Unity;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            this.ConfigBearerToken(services);
            services.AddControllers();
            services.AddMvc(c =>
            {
                c.Filters.Add(new ProducesAttribute("application/json"));
                c.Filters.Add(new SwaggerResponseAttribute(StatusCodes.Status400BadRequest, type: typeof(Error400Response)));
                c.Filters.Add(new SwaggerResponseAttribute(StatusCodes.Status401Unauthorized));
                c.Filters.Add(new SwaggerResponseAttribute(StatusCodes.Status403Forbidden));
                c.Filters.Add(new SwaggerResponseAttribute(StatusCodes.Status404NotFound));
                c.Filters.Add(new SwaggerResponseAttribute(StatusCodes.Status200OK));
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
            })
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddControllersAsServices()
                .AddXmlSerializerFormatters()
            ;
            if (Env.IsDevelopment())
            {
                //ConfigSwagger(services);
            }

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfig());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.Configure<JwtSetting>(Configuration.GetSection("JwtSetting"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<NapTheNgaySetting>(Configuration.GetSection("NapTheNgay"));
            services.Configure<SftpSetting>(Configuration.GetSection("SftpSetting"));
            services.Configure<CryptoSettings>(Configuration.GetSection("CryptoSettings"));
            services.Configure<ConnectionSetting>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<WheelSetting>(Configuration.GetSection("WheelSetting"));
            services.Configure<List<StatisticSetting>>(Configuration.GetSection("Statistics"));
            services.AddScoped<DatabaseContext>();
            services.AddSingleton<DiscordSocketClient>();
            services.AddScoped<ILoggerManager, LoggerHelper>();
            // Register interface
            this.Register(services);
            this.LoggerConfigure();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c =>
                //{
                //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                //    c.RoutePrefix = string.Empty;
                //});
                //app.UseMiddleware<AutomationLoginMiddleware>();
            }
            app.UseCors("MyPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMetricServer();
            app.UseHttpMetrics();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
                endpoints.MapControllers();
            });
            
            
            app.UseStaticFiles();

        }

        #region Configuration
        public void ConfigureContainer(IUnityContainer container)
        {
            
        }

        private void ConfigSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "1",
                    Title = "API V1",
                    Description = "Đây là 1 project code structure"
                });
                c.ResolveConflictingActions(a => a.First());

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath =
                    Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                c.IncludeXmlComments(xmlCommentsFullPath);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Sau khi gọi API Login thì sẽ được cấp JWT, điền nó vào đây",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });
        }
        private void ConfigBearerToken(IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new AuthorationAttribute();
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("UjREy6FyOhFANL4dX7WFxX5LOBMVPpis")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });
        }
        private void Register(IServiceCollection services)
        {
            services.AddScoped(typeof(IGeneralService<>), typeof(GeneralService<>));
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<DatabaseContext>();
            var ass = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.ImplementedInterfaces.Any());
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
        private void LoggerConfigure()
        {
            XmlDocument log4netConfig = new XmlDocument();

            using (var fs = File.OpenRead("log4net.config"))
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
