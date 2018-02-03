using System;
using System.Collections.Generic;

namespace HueLogging.Models.Interfaces
{
	public interface IHueLoggingRepo : IDisposable
	{

		/// <summary>
		/// Get last Config
		/// </summary>
		/// <returns></returns>
		HueConfigStates GetRecentConfig();

		/// <summary>
		/// Get all the events since a duration back
		/// </summary>
		/// <returns></returns>
		IEnumerable<LightEvent> GetRecentEvents(TimeSpan durationBack);

		/// <summary>
		/// Get the number of events
		/// </summary>
		/// <returns></returns>
		IEnumerable<LightEvent> GetLastNumberOfEvents();

		/// <summary>
		/// Get the event by the Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		LightEvent GetEvent(int id);

		/// <summary>
		/// Get the last event recorded
		/// </summary>
		/// <returns></returns>
		LightEvent GetLastEvent();

		/// <summary>
		/// Get the last event recoded for a particular light id
		/// </summary>
		/// <param name="lightId"></param>
		/// <returns></returns>
		LightEvent GetLastEvent(string lightId);

		/// <summary>
		/// Save a single light event
		/// </summary>
		void Save(LightEvent lightEvent);

		/// <summary>
		/// Save multiple light events in one go
		/// </summary>
		/// <param name="lightEvent"></param>
		void Save(IEnumerable<LightEvent> lightEvent);

		/// <summary>
		/// Add Config Changes
		/// </summary>
		/// <param name="configStates"></param>
		void Save(HueConfigStates configStates);
	}
}
