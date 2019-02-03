using System.ComponentModel.DataAnnotations;

namespace HueLogging.Standard.Models
{
	/// <summary>
	/// Information about a Hue Light
	/// </summary>
	public class Light
	{
		/// <summary>
		/// Identity
		/// </summary>
		/// <value></value>
		public string Id { get; set; }
		
		/// <summary>
		/// Hue Type
		/// </summary>
		/// <value></value>
		public string HueType { get; set; }
		
		/// <summary>
		/// User friendly Identity
		/// </summary>
		/// <value></value>
		public string Name { get; set; }
		
		/// <summary>
		/// Model Id
		/// </summary>
		/// <value></value>
		public string ModelId { get; set; }
		
		/// <summary>
		/// Software version
		/// </summary>
		/// <value></value>
		public string SWVersion { get; set; }
	}
}
