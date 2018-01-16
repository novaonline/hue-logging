using HueLogging.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.DAL.Api
{
    public interface IHueAccess
    {
		/// <summary>
		/// to determine if hue has been active since the date specified.
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		bool HasBeenActiveSince(DateTime dateTime);

		/// <summary>
		/// get all the current light information.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Light> GetLights();

		/// <summary>
		/// get the current light by Id
		/// </summary>
		/// <returns></returns>
		Light GetLight(uint id);
	}
}
