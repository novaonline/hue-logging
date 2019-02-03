using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using HueLogging.Standard.Source.Cassandra.Datastax;
using HueLogging.Standard.Source.Cassandra.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HueLogging.Standard.Source.Cassandra.Extensions
{
	public static class HueLoggingServiceSourceConfiguration
	{
		public static void AddCassandraSourceToHueLogging(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<ICassandraDriver, DatastaxCassandraDriver>();
			serviceCollection.AddSingleton<IHueLoggingListingSource<LightKey>, LightKeyListingCassandraSource>();
			serviceCollection.AddSingleton<IHueLoggingSource<LightEvent>, LightEventCassandraSource>();
			serviceCollection.AddSingleton<IHueLoggingSource<LightSession>, LightSessionCassandraSource>();
			serviceCollection.AddSingleton<IHueLoggingSource<LightSummary>, LightSummaryCassandraSource>();
		}

	}
}
