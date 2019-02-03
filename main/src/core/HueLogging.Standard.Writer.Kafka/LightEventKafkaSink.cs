using Confluent.Kafka;
using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Sink.Kafka
{
	public class LightEventKafkaSink : IHueLoggingSink<LightEvent>
	{
		private readonly ILogger<LightEventKafkaSink> _logger;
		private readonly ProducerConfig _producerConfig;
		private readonly IBasicBinarySerializer _serializer;
		private const string LIGHT_EVENT_TOPIC = "hue-logging-light-event";

		public LightEventKafkaSink(IConfiguration configuration, ILogger<LightEventKafkaSink> logger, IBasicBinarySerializer serializer)
		{
			_logger = logger;
			_producerConfig = new ProducerConfig
			{
				BootstrapServers = configuration["HueLogging:Kafka:BootstrapServers"],
			};
			_serializer = serializer;
		}

		public async Task Save(LightEvent lightEvent)
		{
			using (var p = new Producer<Null, byte[]>(_producerConfig))
			{
				try
				{
					var dr = await p.ProduceAsync(LIGHT_EVENT_TOPIC, new Message<Null, byte[]> { Value = _serializer.To(lightEvent) });
					_logger.LogInformation($"Delivered '{dr.TopicPartitionOffset}'");
				}
				catch (KafkaException ex)
				{
					_logger.LogError($"Delivery failed: {ex.Error.Reason}");
				}
			}
		}

		public async Task Save(IEnumerable<LightEvent> lightEvent)
		{
			foreach (var l in lightEvent)
			{
				await Save(l);
			}
		}

	}
}
