using HueLogging.Standard.Library;
using HueLogging.Standard.Models.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLogging.Service
{
	public class Job : IJob
	{
		private IHueLoggingManager hueLoggingManager;

		private static int counter;


		public Job()
		{
		}

		public void Execute(IJobExecutionContext context)
		{
			try
			{
				Console.WriteLine(counter++);
				Program.hueLoggingManager.Start(bool.Parse(Program.arguments));
			}
			catch (Exception ex)
			{
				throw ex;
			}

			Program.arguments = "false";
		}
	}
}
