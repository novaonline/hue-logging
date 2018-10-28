using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Models.Interfaces
{
	// TODO: Splitup the config
	public interface IHueLoggingWriter 
	{
		/// <summary>
		/// Save a single light event
		/// </summary>
		Task Save(LightEvent lightEvent);

		/// <summary>
		/// Save multiple light events in one go
		/// </summary>
		/// <param name="lightEvent"></param>
		Task Save(IEnumerable<LightEvent> lightEvent);
	}
}
