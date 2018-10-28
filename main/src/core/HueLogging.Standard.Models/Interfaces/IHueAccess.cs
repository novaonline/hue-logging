using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Models.Interfaces
{
	/// <summary>
	/// This should never write to the Hue. Any Activities done here is ignored when logging.
	/// </summary>
	public interface IHueAccess
    {
		/// <summary>
		/// Get the Bridge IP for setup
		/// </summary>
		/// <returns></returns>
		Task<string> GetBridgeIpForSetup();

		/// <summary>
		/// Get the AppKey for setup
		/// </summary>
		/// <returns></returns>
		Task<string> GetAppKeyForSetup(string ip);

		/// <summary>
		/// Setting up anything needed before accessing.
		/// </summary>
		Task<(string IpAddress, string Key)> Setup();

		/// <summary>
		/// to determine if hue has been active since the date specified (UTC).
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		Task<bool> HasBeenActiveSince(DateTimeOffset dateTime);

		/// <summary>
		/// get all the current light information.
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<LightEvent>> GetLights();

		/// <summary>
		/// get the current light by Id
		/// </summary>
		/// <returns></returns>
		Task<LightEvent> GetLight(string id);
	}
}
