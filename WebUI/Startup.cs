using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using WebUI.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebUI.Services;
using Hangfire;
using WebUI.Health;

namespace WebUI
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
            services.Configure<Helpers.MailSettings>(Configuration.GetSection("MailSettings"));


            var _dbConnectionString = Configuration.GetConnectionString("DowntimeDbConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_dbConnectionString)
            );

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddTransient<ApplicationDbContext>();
            services.AddScoped<ITargetAppService, TargetAppService>();
            services.AddScoped<INotifyService, MailService>();
            services.AddScoped<ILogService, LogService>();

            services.AddRazorPages();

            services.AddMvc()
              .AddJsonOptions(o =>
              {
                  o.JsonSerializerOptions.PropertyNamingPolicy = new WebUI.Helpers.JsonPascalCaseNamingPolicy();
              });

            services.AddHangfire(x => x.UseSqlServerStorage(_dbConnectionString));

            HealthCheckConfiguration.RegisterHealthCheckSettings(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            HealthCheckConfiguration.ConfigureHealthChecks(app, env);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware<Middlewares.ExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseHangfireDashboard(); // http://localhost/hangfire
            app.UseHangfireServer();
        }

        
    }
}
