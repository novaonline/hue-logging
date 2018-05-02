using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.Standard.Models
{
	public class HueSession
    {
		public int Id { get; set; }
		public string LightId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public Light Light { get; set; }
	}
}
