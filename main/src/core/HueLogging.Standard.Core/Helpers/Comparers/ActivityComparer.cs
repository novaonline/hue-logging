using HueLogging.Standard.Models;

namespace HueLogging.Standard.Library.Helpers.Comparers
{
	public class ActivityComparer : OnStateComparer
	{
		public override int Compare(LightEvent x, LightEvent y)
		{
			return base.Compare(x, y) == 0 ? CompareSatState(x.State, y.State) : base.Compare(x, y);
		}

		private int CompareSatState(LightState x, LightState y) => x.Saturation.CompareTo(y.Saturation) == 0
			? CompareBriState(x, y)
			: x.Saturation.CompareTo(y.Saturation);

		private int CompareBriState(LightState x, LightState y) => x.Brightness.CompareTo(y.Brightness) == 0
			? CompareHueState(x, y)
			: x.Brightness.CompareTo(y.Brightness);

		private int CompareHueState(LightState x, LightState y) => x.Hue.CompareTo(y.Hue) == 0
			? 0
			: x.Saturation.CompareTo(y.Saturation);
	}
}
