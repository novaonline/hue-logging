using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HueLogging.Standard.Models
{
	/// <summary>
	/// Information about a light's state 
	/// </summary>
	public class LightState
	{
		/// <summary>
		/// Flag identifying if the light was on or off
		/// </summary>
		/// <value></value>
		public bool On { get; set; }

		/// <summary>
		/// The light's brightness value
		/// </summary>
		/// <value></value>
		public short Brightness { get; set; }

		/// <summary>
		/// The light's saturation value
		/// </summary>
		/// <value></value>
		public short Saturation { get; set; }

		/// <summary>
		/// The light's Hue value
		/// </summary>
		/// <value></value>
		public int Hue { get; set; }

		/// <summary>
		/// Flag identifying if the light was reachable or not		
		/// </summary>
		/// <value></value>
		public bool Reachable { get; set; }

		/// <summary>
		/// Add Date
		/// </summary>
		public DateTimeOffset? AddDate { get; set; }
	}
}
