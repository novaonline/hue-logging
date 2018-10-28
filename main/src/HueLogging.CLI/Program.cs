using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
					try
					{
						Task.WaitAll(serviceProvider.GetService<App>().Setup());
						return 0;
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
						return -1;
					}

				});
			});
			cliApp.Execute(args);
		}

		private static void ConfigureServices(IServiceCollection serviceCollection)
		{

			IConfiguration configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false)
				.Build();

			serviceCollection.AddSingleton(configuration);
			serviceCollection.AddLogging(c => c.AddConsole());
			serviceCollection.AddTransient<App>();
		}
	}



}
