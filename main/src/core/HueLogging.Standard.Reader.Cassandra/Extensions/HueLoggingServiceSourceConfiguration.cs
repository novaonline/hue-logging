using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HueLogging.Standard.Source.Cassandra.Extensions
{
	public static class HueLoggingServiceSourceConfiguration
	{
		public static void AddCassandraSourceToHueLogging(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IHueLoggingSource<LightEvent>, LightEventCassandraSource>();
		}

	}
}
