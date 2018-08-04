using HueLogging.Standard.Models.Interfaces;
using System;
using System.Threading.Tasks;

namespace HueLogging.CLI
{
	public class App
	{
		private readonly IHueAccess hueAccess;

		public App(IHueAccess hueAccess)
		{
			this.hueAccess = hueAccess;
		}

		public void Setup()
		{
			Console.Write("This setup requires two things. HueLoggingDatabase configured and access to the hue bridge button. Are you ready to setup? (y/n): ");
			if (Console.ReadLine().Equals("y"))
			{
				hueAccess.Setup();
			}
			else
			{
				Console.WriteLine("Try command again when you are ready. Bye.");
			}
		}
	}
}
