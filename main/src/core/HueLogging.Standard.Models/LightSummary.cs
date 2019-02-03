using System;

namespace HueLogging.Standard.Models
{
	/// <summary>
	/// Aggregated Information for a particular light
	/// </summary>
	public class LightSummary
	{
		/// <summary>
		/// How long the light has been on from the time the application started logging
		/// </summary>
		/// <value></value>
		public long DurationInSeconds { get; set; }
		
		/// <summary>
		/// The most recent light information
		/// </summary>
		/// <value></value>
		public Light Light { get; set; }

		/// <summary>
		/// The processing date 
		/// </summary>
		/// <value></value>
		public DateTimeOffset AddDate { get; set; }
	}
}
