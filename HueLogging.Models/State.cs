using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.Models
{
    public class State
    {
		public bool On { get; set; }
		public UInt16 Brightness { get; set; }
		public UInt16 Saturation { get; set; }
		public UInt16 Hue { get; set; }
		public bool Reachable { get; set; }
	}
}
