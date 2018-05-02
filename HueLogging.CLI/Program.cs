using HueLogging.Standard.Library.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HueLogging.CLI
{
	public class Program
	{
		static void Main(string[] args)
		{
			var shouldReset = false;
			if (args.Length > 0) bool.TryParse(args[0], out shouldReset);
			var services = new ServiceCollection();
			ConfigureServices(services);
			var serviceProvider = services.BuildServiceProvider();
			serviceProvider.GetService<App>().Run(shouldReset);
		}

		private static void ConfigureServices(IServiceCollection serviceCollection)
		{
			// TODO: Logging

			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false)
				.AddJsonFile("appsettings.Development.json", true)
				.Build();
			serviceCollection.AddSingleton(new LoggerFactory()
				.AddConsole());
			serviceCollection.AddLogging();
			serviceCollection.AddHueLogging(options => options.UseSqlServer(configuration.GetConnectionString("HueLoggingConnection")));

			serviceCollection.AddTransient<App>();
		}
	}



}
