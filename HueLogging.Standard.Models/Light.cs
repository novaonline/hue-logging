using System.ComponentModel.DataAnnotations;

namespace HueLogging.Standard.Models
{
	public class Light
    {
		[Key]
		public string Id { get; set; }
		
		public string HueType { get; set; }
		public string Name { get; set; }
		public string ModelId { get; set; }
		public string SWVersion { get; set; }
	}
}
