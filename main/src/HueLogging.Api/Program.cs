﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HueLogging.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();
			host.Run();
		}

	}
}
