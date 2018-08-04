using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HueLogging.Standard.Library.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HueLogging.ServiceV2
{
    class Program
    {
        static async Task Main(string[] args)
        {
			var host = new HostBuilder()
				.ConfigureHostConfiguration(configHost =>
				{
					configHost.SetBasePath(Directory.GetCurrentDirectory());
					configHost.AddJsonFile("hostsettings.json", optional: true);
					configHost.AddEnvironmentVariables(prefix: "PREFIX_");
					configHost.AddCommandLine(args);
				})
				.ConfigureAppConfiguration((hostContext, configApp) =>
				{
					configApp.AddJsonFile("appsettings.json", optional: true);
					configApp.AddJsonFile(
						$"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
						optional: true);
					configApp.AddEnvironmentVariables(prefix: "HueLogging_");
					configApp.AddCommandLine(args);
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddLogging();
					services.AddHostedService<LifetimeEventsHostedService>();
					services.AddHostedService<HueLoggingRecurringService>();
					if(hostContext.HostingEnvironment.IsDevelopment())
					{
						services.AddHueLogging(options => options.UseInMemoryDatabase("HueLoggingConnection"));
					}
					else
					{
						services.AddHueLogging(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("HueLoggingConnection")));
					}
				})
				.ConfigureLogging((hostContext, configLogging) =>
				{
					configLogging.AddConsole();
					configLogging.AddDebug();
				})
				.UseConsoleLifetime()
				.Build();

			await host.RunAsync();
		}
    }
}
