using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Models.Interfaces
{
	/// <summary>
	/// The Hue Logging Streaming Source
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IHueLoggingStreamSource<T> where T : object
	{
		Task Subscribe(string lightName, int limit = 100);
	}
}
