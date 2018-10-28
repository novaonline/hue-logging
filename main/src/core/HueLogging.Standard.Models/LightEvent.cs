using System;
using System.ComponentModel.DataAnnotations;

namespace HueLogging.Standard.Models
{
	public class LightEvent
    {
		[Key]
		public int Id { get; set; }
		public Light Light { get; set; }
		public State State { get; set; }
		public DateTimeOffset AddDate { get; set; }
	}
}
