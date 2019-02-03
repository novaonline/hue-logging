using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Source.Kafka
{
	public class LightEventKafkaStream : IHueLoggingSource<LightEvent>
	{
		public Task<IEnumerable<LightEvent>> GetRecentByLightName(string lightName, int limit = 100)
		{
			throw new NotImplementedException();
		}
	}
}
