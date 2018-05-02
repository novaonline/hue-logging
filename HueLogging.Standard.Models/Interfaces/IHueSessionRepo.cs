using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HueLogging.Standard.Models.Interfaces
{
    public interface IHueSessionRepo
    {
		/// <summary>
		/// Get the last incomplete by light id
		/// </summary>
		/// <param name="lightId"></param>
		/// <returns></returns>
		HueSession GetLastIncompleteSession(string lightId);

		/// <summary>
		/// Get Sessions where start date is in between Start Date and End Date
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		IEnumerable<HueSession> GetHueSessions(DateTime startDate, DateTime endDate);

		/// <summary>
		/// Get the hue session summary for each light
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		IEnumerable<HueSessionSummary> GetHueSessionSummary(DateTime startDate, DateTime endDate);

		/// <summary>
		/// Add or Delete hue Session. Make sure ID == 0 if you want to insert
		/// </summary>
		/// <param name="hueSession"></param>
		void Save(HueSession hueSession);
    }
}
