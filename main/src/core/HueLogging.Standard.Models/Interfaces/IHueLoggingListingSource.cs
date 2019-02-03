using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Models.Interfaces
{
	/// <summary>
	/// The Hue Logging Source for listing
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IHueLoggingListingSource<T> where T : object
	{
		Task<IEnumerable<T>> GetAll(int limit = 100);
	}
}
