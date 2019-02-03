using System;

namespace HueLogging.Standard.Models
{
	public class LightSession
	{
		public Light Light { get; set; }
		public State StartState { get; set; }
		public State EndState { get; set; }
		public long DurationInSeconds { get; set; }
		public DateTimeOffset AddDate { get; set; }
	}
}
