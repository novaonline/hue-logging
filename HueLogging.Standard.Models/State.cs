using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HueLogging.Standard.Models
{
	public class State
    {
		[Key]
		public int Id { get; set; }
		public bool On { get; set; }
		public short Brightness { get; set; }
		public short Saturation { get; set; }
		public int Hue { get; set; }
		public bool Reachable { get; set; }
	}
}
