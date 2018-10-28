using HueLogging.CLI.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HueLogging.CLI
{
	public class App
	{
		private readonly IHueAccess hueAccess;
		private readonly ILogger<App> logger;

		public App(IHueAccess hueAccess, ILogger<App> logger)
		{
			this.hueAccess = hueAccess;
			this.logger = logger;
		}

		public async Task Setup()
		{
			Console.Write("This setup requires an API key. Let's get access to the hue bridge button. Are you ready to setup? (y/n): ");
			if (Console.ReadLine().Equals("y"))
			{
				var (IpAddress, Key) = await hueAccess.Setup();
				File.WriteAllText("hue-key.json", Key);
				logger.LogInformation($"Please set environment variable 'HueLogging:ApiKey={Key}' ");
				logger.LogInformation("Your key has been written to hue-key.json in current directory.");
				logger.LogWarning("You'll also need to setup enivronment variable 'HueLogging:Kafka:BootstrapServers'");
			}
			else
			{
				Console.WriteLine("Try command again when you are ready. Bye.");
			}
		}
	}
}
