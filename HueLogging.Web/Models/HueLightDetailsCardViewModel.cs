using System.Collections.Generic;

namespace HueLogging.Web.Models
{
	public class HueLightDetailsCardViewModel : BootstrapCardViewModel
    {
		public IEnumerable<Standard.Models.Light> Lights { get; set; }
	}
}
