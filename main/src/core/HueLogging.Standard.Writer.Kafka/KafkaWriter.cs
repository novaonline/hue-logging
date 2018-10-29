using Confluent.Kafka;
using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Sink.Kafka
{
	public class KafkaWriter : IHueLoggingWriter
	{
		private readonly ILogger<KafkaWriter> _logger;
		private readonly ProducerConfig _producerConfig;

		public KafkaWriter(IConfiguration configuration, ILogger<KafkaWriter> logger)
		{
			_logger = logger;
			_producerConfig = new ProducerConfig
			{
				
				BootstrapServers = configuration["HueLogging:Kafka:BootstrapServers"],
				
			};
		}

		public async Task Save(LightEvent lightEvent)
		{
			using (var p = new Producer<Null, string>(_producerConfig))
			{
				try
				{
					var value = JsonConvert.SerializeObject(lightEvent);
					var dr = await p.ProduceAsync("hue-logging-light-event", new Message<Null, string> { Value = value });
					_logger.LogInformation($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
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
