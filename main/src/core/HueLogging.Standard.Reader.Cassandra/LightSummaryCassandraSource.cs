using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using HueLogging.Standard.Source.Cassandra.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Source.Cassandra
{
	public class LightSummaryCassandraSource : IHueLoggingSource<LightSummary>
	{
		private readonly ICassandraDriver _driver;

		public LightSummaryCassandraSource(ICassandraDriver driver, ILogger<LightSummaryCassandraSource> logger)
		{
			_driver = driver;
		}

		public async Task<IEnumerable<LightSummary>> GetRecentByLightName(string lightName, int limit = 100) => await _driver.QueryAsync<LightSummary>("FROM light_total_duration_by_light_name WHERE light_name = ?", limit, lightName);
	}
}
