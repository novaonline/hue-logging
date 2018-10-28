using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueLogging.Api.Models.Request.Filters
{
    public class SessionFilter : BaseFilter
    {
		public string LightName { get; set; }
    }
}
