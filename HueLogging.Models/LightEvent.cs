using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HueLogging.Models
{
    public class LightEvent
    {
		[Key]
		public uint Id { get; set; }
		public Light Light { get; set; }
		public DateTime AddDate { get; set; }
	}
}
