using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.Models
{
    public class HueSetupOptions
    {
		public static int WaitPeriodInMs { get; set; } = 2000;
		public static int MaxAttempts { get; set; } = 10;
	}
}
