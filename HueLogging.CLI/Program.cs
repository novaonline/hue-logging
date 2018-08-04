using HueLogging.Standard.Library.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HueLogging.CLI
{
	/// <summary>
	/// This will be redone to aid the setup process. In particular, getting the API key from Hue bridge
	/// </summary>
	public class Program
	{
		static void Main(string[] args)
		{
			var services = new ServiceCollection();
			ConfigureServices(services);
			var serviceProvider = services.BuildServiceProvider();

			var cliApp = new CommandLineApplication
			{
				Name = "hue-log"
			};
			cliApp.HelpOption("-?|-h|--help");

			cliApp.Command("setup", (command) =>
			{
				command.Description = "Setup hue logging's api key";
				command.HelpOption("-?|-h|--help");
				command.OnExecute(() =>
				{
					serviceProvider.GetService<App>().Setup();
					return 0;
				});
			});
			cliApp.Execute(args);
			//serviceProvider.GetService<App>().Run(shouldReset);
		}

		private static void ConfigureServices(IServiceCollection serviceCollection)
		{
			IConfiguration configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false)
				.AddJsonFile("appsettings.Development.json", true)
				.Build();

			serviceCollection.AddSingleton<IConfiguration>(configuration);

			serviceCollection.AddSingleton(new LoggerFactory()
				.AddConsole());
			serviceCollection.AddLogging();
			serviceCollection.AddHueLogging(options => options.UseSqlServer(configuration.GetConnectionString("HueLoggingConnection")));
			serviceCollection.AddTransient<App>();
		}
	}



}
