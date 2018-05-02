using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Web.Models
{
    public class StatusCardViewModel : BootstrapCardViewModel
    {
		public HueLoggingStatus Status { get; set; }
	}

	public enum HueLoggingStatus
	{
		UNKNOWN = 0,
		RUNNING = 1,
		STOPPED = 2
	}
}
