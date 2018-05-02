using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using HueLogging.Standard.Models.Interfaces;

namespace HueLogging.Service
{
	public class App
	{
		public App(string exePath, string arguments, int delaySeconds)
		{
			Program.exePath = exePath;
			Program.delayMilliseconds = delaySeconds * 1000;
			Program.arguments = arguments;
		}

		public void Start()
		{
			Console.WriteLine("Started");
			//reoccur = new Timer(Execute, null, 0, delayMilliseconds);
		}

		public void Stop()
		{
			//reoccur.Dispose();
		}

	}
}
