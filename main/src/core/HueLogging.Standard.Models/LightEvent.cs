using System;
using System.ComponentModel.DataAnnotations;

namespace HueLogging.Standard.Models
{
	/// <summary>
	/// A model that captures a light event
	/// </summary>
	public class LightEvent
    {
		/// <summary>
		/// The Light Info when the event was captured
		/// </summary>
		/// <value></value>
		public Light Light { get; set; }
		
		/// <summary>
		/// The state when the light event was captured
		/// </summary>
		/// <value></value>
		public LightState State { get; set; }

		/// <summary>
		/// The date and time the event was captured
		/// </summary>
		/// <value></value>
		public DateTimeOffset AddDate { get; set; }
	}
}
