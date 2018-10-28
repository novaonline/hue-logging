using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Api.Models.Helpers
{
    public class TimeFrame
    {
		[Range(1, 365, ErrorMessage = "Days back must be between 1 day and 1 year")]
		public int DaysBack { get; set; }
	}
}
