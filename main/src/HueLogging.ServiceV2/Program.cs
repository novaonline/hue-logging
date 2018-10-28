using HueLogging.Standard.Library.Extensions;
using HueLogging.Standard.Sink.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HueLogging.ServiceV2
{
	class Program
    {
        static async Task Main(string[] args)
        {
			var host = new HostBuilder()
				.ConfigureAppConfiguration((hostContext, configApp) =>
				{
					configApp.AddJsonFile("appsettings.json", optional: true);
					configApp.AddEnvironmentVariables();
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
