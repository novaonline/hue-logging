using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.Standard.Models
{
	public class HueSession
    {
		public int Id { get; set; }
		public string LightId { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset? EndDate { get; set; }

		public Light Light { get; set; }
	}
}
