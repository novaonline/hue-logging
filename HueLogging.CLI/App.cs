using HueLogging.Standard.Models.Interfaces;
using System;
using System.Threading.Tasks;

namespace HueLogging.CLI
{
	public class App
    {
		private readonly IHueLoggingManager hueLoggingManager;

		public App(IHueLoggingManager hueLoggingManager)
		{
			this.hueLoggingManager = hueLoggingManager;
		}

		public void Run(bool shouldStartNewSession = false)
		{
			Console.WriteLine($"Started with new session as {shouldStartNewSession}");
			hueLoggingManager.Start(shouldStartNewSession: shouldStartNewSession);
			Console.WriteLine($"Done with new session as {shouldStartNewSession}");
			Task.Delay(1000).Wait();

		}
	}
}
