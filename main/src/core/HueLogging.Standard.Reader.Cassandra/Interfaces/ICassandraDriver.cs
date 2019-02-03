using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Source.Cassandra.Interfaces
{
	/// <summary>
	/// The required functions required when using a Cassandra driver.
	/// </summary>
	public interface ICassandraDriver
	{


		/// <summary>
		/// Get Results from query string and passing obj params.
		/// </summary>
		/// <typeparam name="R"></typeparam>
		/// <param name="query"></param>
		/// <param name="p"></param>
		/// <returns></returns>
		Task<IEnumerable<R>> QueryAsync<R>(string query, int pageSize, params object[] p);
	}
}
