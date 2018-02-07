using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.Models
{
    public class HueSessionSummary
    {
		public TimeSpan TotalDuration { get; set; }
		public Light Light { get; set; }
	}
}
