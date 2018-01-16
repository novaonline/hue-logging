using HueLogging.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HueLogging.Library
{
    interface ILoggingManager
    {
		/// <summary>
		/// Begin
		/// </summary>
		void Start();

		/// <summary>
		/// If precondition has failed log event should not continue.
		/// TODO: This one doesnt seem to make sense
		/// </summary>
		/// <returns></returns>
		bool HasPassedPreCondition();

		/// <summary>
		/// Log the event
		/// </summary>
		void LogEvent();
    }
}
