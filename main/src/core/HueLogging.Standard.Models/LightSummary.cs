using System;

namespace HueLogging.Standard.Models
{
	public class LightSummary
	{
		public long DurationInSeconds { get; set; }
		public Light Light { get; set; }
		public DateTimeOffset AddDate { get; set; }
	}
}
