using System;
using System.Collections.Generic;

namespace HueLogging.Standard.Models.Interfaces
{
	/// <summary>
	/// This should never write to the Hue. Any Activities done here is ignored when logging.
	/// </summary>
	public interface IHueAccess
    {
		/// <summary>
		/// Persist the configuration
		/// </summary>
		/// <returns></returns>
		HueConfigStates PersistConfig(HueConfigStates config);

		/// <summary>
		/// Get the Bridge IP for setup
		/// </summary>
		/// <returns></returns>
		string GetBridgeIpForSetup();

		/// <summary>
		/// Get the AppKey for setup
		/// </summary>
		/// <returns></returns>
		string GetAppKeyForSetup(string ip);

		/// <summary>
		/// Setting up anything needed before accessing.
		/// </summary>
		void Setup();


		/// <summary>
		/// to determine if hue has been active since the date specified (UTC).
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		bool HasBeenActiveSince(DateTime dateTime);

		/// <summary>
		/// get all the current light information.
		/// </summary>
		/// <returns></returns>
		IEnumerable<LightEvent> GetLights();

		/// <summary>
		/// get the current light by Id
		/// </summary>
		/// <returns></returns>
		LightEvent GetLight(string id);
	}
}
