using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HueLogging.Standard.Sink.Kafka.Extensions
{
	public static class HueLoggingServiceSinkConfiguration
	{
		public static void AddHueLoggingWithKafkaSink(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IHueLoggingWriter, KafkaWriter>();
		}
	}
}
