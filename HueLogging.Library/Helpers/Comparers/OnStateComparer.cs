﻿using HueLogging.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.Library.Helpers.Comparers
{
	public class OnStateComparer : Comparer<LightEvent>
	{
		public override int Compare(LightEvent x, LightEvent y)
		{
			if (x == null) return -1;
			if (y == null) return 1;
			if (x == null || x.State == null || y == null || y.State == null) return 0;

			return CompareOnState(x.State, y.State);
		}

		private int CompareOnState(State x, State y) => x.On.CompareTo(y.On);
	}
}
