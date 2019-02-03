using System;
using System.Collections.Generic;

namespace HueLogging.Standard.Models
{
    /// <summary>
    /// A Light Session which contains a list of states from the time a light was on all the way to an off state.
    /// </summary>
    public class LightSession
    {
        /// <summary>
        /// The most recent Light Information during a session
        /// </summary>
        /// <value></value>
        public Light Light { get; set; }

        /// <summary>
        /// The list of states ordered by event time during a session
        /// </summary>
        /// <value></value>
        public List<LightState> States { get; set; }

        /// <summary>
        /// How long (in seconds) the light was on for
        /// </summary>
        /// <value></value>
        public long DurationInSeconds { get; set; }

        /// <summary>
        /// The processing date
        /// </summary>
        /// <value></value>
        public DateTimeOffset AddDate { get; set; }
    }
}
