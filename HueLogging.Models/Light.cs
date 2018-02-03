using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HueLogging.Models
{
    public class Light
    {
		[Key]
		public string Id { get; set; }
		
		public string HueType { get; set; }
		public string Name { get; set; }
		public string ModelId { get; set; }
		public string SWVersion { get; set; }
	}
}
