using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using HueLogging.Standard.Source.Cassandra.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Source.Cassandra
{
	public class LightKeyListingCassandraSource : IHueLoggingListingSource<LightKey>
	{
		private readonly ICassandraDriver _driver;

		public LightKeyListingCassandraSource(ICassandraDriver driver, ILogger<LightKeyListingCassandraSource> logger)
		{
			_driver = driver;
		}

		public async Task<IEnumerable<LightKey>> GetAll(int limit = 1000) => await _driver.QueryAsync<LightKey>("SELECT DISTINCT light_name FROM light_total_duration_by_light_name", limit);
	}
}
