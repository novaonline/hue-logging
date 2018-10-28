using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.Standard.Models
{
    public class HueSetupOptions
    {
		public static int WaitPeriodInMs { get; set; } = 5000;
		public static int MaxAttempts { get; set; } = 10;
		public static string Dns { get; set; } = "philips-hue.lan";
	}
}
