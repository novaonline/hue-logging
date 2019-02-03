using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Models.Interfaces
{
	// TODO: Splitup the config
	public interface IHueLoggingSink<T>
	{
		/// <summary>
		/// Save a single light event
		/// </summary>
		Task Save(T lightEvent);

		/// <summary>
		/// Save multiple light events in one go
		/// </summary>
		/// <param name="lightEvent"></param>
		Task Save(IEnumerable<T> lightEvent);
	}
}
