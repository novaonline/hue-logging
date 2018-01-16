using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HueLogging.Models
{
    public class Light
    {
		[Key]
		public uint Id { get; set; }
		public State State { get; set; }
		public string HueType { get; set; }
		public string Name { get; set; }
		public string ModelId { get; set; }
		public string SWVersion { get; set; }
	}
}
