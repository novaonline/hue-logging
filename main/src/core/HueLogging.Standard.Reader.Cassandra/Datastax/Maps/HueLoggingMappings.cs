using Cassandra;
using Cassandra.Mapping;
using HueLogging.Standard.Models;

namespace HueLogging.Standard.Source.Cassandra.Datastax.Maps
{
	public class HueLoggingMappings : Mappings
	{
		public HueLoggingMappings()
		{
			MapLightEvent();
			MapLightSession();
			MapLightSummary();
			MapLightKey();
		}

		public static UdtMap[] GetUdtMaps()
		{
			return new UdtMap[] {
				UdtMap.For<Light>("light_info")
				.Map(l => l.Id, "id")
				.Map(l => l.HueType, "hue_type")
				.Map(l => l.Name, "name")
				.Map(l => l.ModelId, "model_id")
				.Map(l => l.SWVersion, "sw_version"),
				UdtMap.For<State>("light_state")
				.Map(s => s.On, "is_on")
				.Map(s => s.Brightness, "brightness")
				.Map(s => s.Saturation, "saturation")
				.Map(s => s.Hue, "hue")
				.Map(s => s.Reachable, "is_reachable")
			};
		}

		public void MapLightEvent()
		{
			For<LightEvent>()
				.Column(l => l.AddDate, cm => cm.WithName("add_date"))
				.Column(l => l.Light, cm => cm.WithName("light"))
				.Column(l => l.State, cm => cm.WithName("state"));
		}

		public void MapLightSession()
		{
			For<LightSession>()
				.Column(ls => ls.AddDate, cm => cm.WithName("add_date"))
				.Column(ls => ls.Light, cm => cm.WithName("light"))
				.Column(ls => ls.StartState, cm => cm.WithName("state_start"))
				.Column(ls => ls.EndState, cm => cm.WithName("start_end"))
				.Column(ls => ls.DurationInSeconds, cm => cm.WithName("duration_in_seconds"));
		}

		public void MapLightSummary()
		{
			For<LightSummary>()
				.Column(lsum => lsum.Light, cm => cm.WithName("light"))
				.Column(lsum => lsum.DurationInSeconds, cm => cm.WithName("duration_in_seconds"))
				.Column(lsum => lsum.AddDate, cm => cm.WithName("add_date"));
		}

		public void MapLightKey()
		{
			For<LightKey>()
				.Column(lk => lk.Name, cm => cm.WithName("light_name"));
		}
	}
}
