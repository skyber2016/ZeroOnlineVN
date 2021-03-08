using AutoMapper;
using Forum_API.Configurations;
using Forum_API.Cores;
using Forum_API.Cores.WebSockets;
using Forum_API.Cores.WebSockets.Handler;
using Forum_API.Database;
using Forum_API.DTO.Error.Responses;
using Forum_API.Helpers;
using Forum_API.Security;
using Forum_API.Services;
using Forum_API.Services.Authenticate;
using Forum_API.Services.Interfaces;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Unity;

namespace Forum_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            ConfigSwagger(services);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfig());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddTransient<ConnectionManager>();
            services.Configure<JwtSetting>(Configuration.GetSection("JwtSetting"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<SftpSetting>(Configuration.GetSection("SftpSetting"));

            services.Configure<ConnectionSetting>(Configuration.GetSection("ConnectionStrings"));
            services.AddDbContext<DatabaseContext>();
            services.AddScoped<ILoggerManager, LoggerHelper>();
            services.AddWebSocketManager();
            // Register interface
            this.Register(services);
            this.LoggerConfigure();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    c.RoutePrefix = string.Empty;
                });
            }
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
            app.UseWebSockets();
            app.MapWebSocketManager("/ws/shoutbox", serviceProvider.GetService<ShoutBoxHandler>());
            app.MapWebSocketManager("/ws/member", serviceProvider.GetService<MemberOnlineHandler>());
            app.MapWebSocketManager("/ws/topics", serviceProvider.GetService<TopicHandler>());
            app.UseCors("MyPolicy");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

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
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(Services.Interfaces.IGeneralService<>), typeof(GeneralService<>));
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
