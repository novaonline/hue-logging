using System;
using System.ComponentModel.DataAnnotations;

namespace HueLogging.Models
{
	public class LightEvent
    {
		[Key]
		public int Id { get; set; }
		public Light Light { get; set; }
		public State State { get; set; }
		public DateTime AddDate { get; set; }
	}
}
