using HueLogging.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using HueLogging.Standard.Library.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HueLogging.Api
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
			services.AddSwaggerGen(options =>
			{
				var info = new Swashbuckle.AspNetCore.Swagger.Info {
					Title = "Hue Logging API",
					Description = "Housing Hue Logging Resources",
					Version = "v1"
				};
				options.SwaggerDoc("v1", info);
			});
			services.AddSingleton(new LoggerFactory().AddConsole());
			services.AddLogging();
			services.AddHueLogging(options => options.UseSqlServer(Configuration.GetConnectionString("HueLoggingConnection")));

			services.AddTransient<LightService>();
			services.AddTransient<EventsService>();
			services.AddTransient<SessionService>();


			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hue Logging API");
			});
			app.UseMvc();
		}
	}
}
