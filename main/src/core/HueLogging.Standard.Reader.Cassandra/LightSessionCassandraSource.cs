using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using HueLogging.Standard.Source.Cassandra.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Source.Cassandra
{
	public class LightSessionCassandraSource : IHueLoggingSource<LightSession>
	{
		private readonly ICassandraDriver _driver;

		public LightSessionCassandraSource(ICassandraDriver driver, ILogger<LightSessionCassandraSource> logger)
		{
			_driver = driver;
		}

		public async Task<IEnumerable<LightSession>> GetRecentByLightName(string lightName, int limit = 100) => await _driver.QueryAsync<LightSession>("FROM light_sessions_by_light_name WHERE light_name = ?", limit, lightName);
	}
}
