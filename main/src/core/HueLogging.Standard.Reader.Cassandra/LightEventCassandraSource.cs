using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using HueLogging.Standard.Source.Cassandra.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Source.Cassandra
{
	public class LightEventCassandraSource : IHueLoggingSource<LightEvent>
	{
		private readonly ICassandraDriver _driver;

		public LightEventCassandraSource(ICassandraDriver driver, ILogger<LightEventCassandraSource> logger)
		{
			_driver = driver;
		}

		public async Task<IEnumerable<LightEvent>> GetRecentByLightName(string lightName, int limit = 100) => await _driver.QueryAsync<LightEvent>("FROM light_events_by_light_name WHERE light_name = ?", limit, lightName);
	}
}
