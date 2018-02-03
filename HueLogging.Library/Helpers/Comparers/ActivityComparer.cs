using System;
using System.Collections.Generic;
using System.Text;
using HueLogging.Models;

namespace HueLogging.Library.Helpers.Comparers
{
	public class ActivityComparer : Comparer<LightEvent>
	{
		public override int Compare(LightEvent x, LightEvent y)
		{
			if (x == null) return -1;
			if (y == null) return 1;
			if (x == null || x.State == null || y == null || y.State == null) return 0;

			return CompareOnState(x.State, y.State);
		}

		private int CompareOnState(State x, State y) => x.On.CompareTo(y.On) == 0
			? CompareSatState(x, y)
			: x.On.CompareTo(y.On);

		private int CompareSatState(State x, State y) => x.Saturation.CompareTo(y.Saturation) == 0
			? CompareBriState(x, y)
			: x.Saturation.CompareTo(y.Saturation);

		private int CompareBriState(State x, State y) => x.Brightness.CompareTo(y.Brightness) == 0
			? CompareHueState(x,y)
			: x.Brightness.CompareTo(y.Brightness);

		private int CompareHueState(State x, State y) => x.Hue.CompareTo(y.Hue) == 0
			? 0
			: x.Saturation.CompareTo(y.Saturation);
	}
}
