using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HueLogging.Standard.Sink.Kafka.Extensions
{
	public static class HueLoggingServiceSinkConfiguration
	{
		public static void AddKafkaSinkToHueLogging(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IHueLoggingSink<LightEvent>, LightEventKafkaSink>();
		}
	}
}
