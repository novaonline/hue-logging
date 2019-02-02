using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HueLogging.Standard.Source.Kafka.Extensions
{
	public static class HueLoggingServiceSourceConfiguration
	{
		public static void AddKafkaStreamSourceToHueLogging(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IHueLoggingSource<LightEvent>, LightEventKafkaStream>();
		}
	}
}
