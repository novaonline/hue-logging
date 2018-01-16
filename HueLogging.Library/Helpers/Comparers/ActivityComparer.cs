using System;
using System.Collections.Generic;
using System.Text;
using HueLogging.Models;

namespace HueLogging.Library.Helpers.Comparers
{
    public class ActivityComparer : Comparer<Light>
    {
		public override int Compare(Light x, Light y)
		{
			if (x == null || x.State == null || y == null || y.State == null) return 0;
			return x.State.On.CompareTo(x.State.On);
		}
	}
}
