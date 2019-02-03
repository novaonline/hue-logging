using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Models.Interfaces
{
	/// <summary>
	/// The Hue Logging Source
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IHueLoggingSource<T> where T : object
	{
		Task<IEnumerable<T>> GetRecentByLightName(string lightName, int limit = 100);
	}
}
