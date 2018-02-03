using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HueLogging.Models.Interfaces;
using HueLogging.Library;
using HueLogging.DAL.Api;
using HueLogging.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace HueLogging.Web
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
			services.AddTransient<ILoggingManager, HueLoggingManager>();
			services.AddTransient<IHueAccess, Q42HueAccess>();
			services.AddTransient<IHueLoggingRepo, HueLogginRepo>();

			services.AddDbContext<HueLoggingContext>(options => options.UseSqlServer(Configuration.GetConnectionString("HueLoggingConnection")));
			services.AddHangfire(options => options.UseSqlServerStorage(Configuration.GetConnectionString("HangFireDBConnection")));

			services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

			// Other pipes here
			app.UseHangfireServer();
			app.UseHangfireDashboard();

			app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
