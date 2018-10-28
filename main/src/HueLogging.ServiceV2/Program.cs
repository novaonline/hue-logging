using HueLogging.Standard.Library.Extensions;
using HueLogging.Standard.Sink.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

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
					configApp.AddJsonFile("appsettings.Dynamic.json", optional: true, reloadOnChange: true);
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

					services.AddHueLogging();
					services.AddHueLoggingWithKafkaSink();
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
