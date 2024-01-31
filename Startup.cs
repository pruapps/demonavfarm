using FarmIT_Api.database_accesslayer;
using FarmIT_Api.Helpers;
using FarmIT_Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FarmIT_Api
{
    public class Startup
    {
        public class ConnectionString
        {
            public string Constr { get; set; }
        }
        public static string ConnectionString_default{ get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
       
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("*")
                                                          .AllowAnyHeader()
                                                          .AllowAnyMethod();
                                  });
            });
            //added by Hare Krishna
            services.AddSession(opction => {
                opction.Cookie.IsEssential = true;
                // opction.IdleTimeout = TimeSpan.FromSeconds(3600);
                opction.IdleTimeout = TimeSpan.FromMinutes(60);
            
            });

            var appsettingsection = Configuration.GetSection("ConnectionString");
            services.Configure<ConnectionString>(appsettingsection);
            var appsetting = appsettingsection.Get<ConnectionString>();

            ConnectionString_default = appsetting.Constr;

            RepositoryService(services,appsetting);
        }
        public static void RepositoryService(IServiceCollection services,ConnectionString appSetting)
        {
            services.AddSingleton<IConfigDB>(repo => new ConfigDB(appSetting.Constr));
            services.AddSingleton<IloginDB>(repo1 => new loginDB(appSetting.Constr));
            services.AddSingleton<IDataentryDB>(repo2 => new DataentryDB(appSetting.Constr));
            services.AddSingleton<IMasterDB>(repo3 => new MasterDB(appSetting.Constr));
            services.AddSingleton<IVoucherDB>(repo4 => new VoucherDB(appSetting.Constr));
            services.AddSingleton<IReportDB>(repo5 => new ReportDB(appSetting.Constr));
            services.AddSingleton<IDashBoardDB>(repo6 => new DashBoardDB(appSetting.Constr));
            services.AddSingleton<ISettingDB>(repo7 => new SettingDB(appSetting.Constr));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
