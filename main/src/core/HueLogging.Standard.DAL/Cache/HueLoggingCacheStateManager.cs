using HueLogging.Standard.Models;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace HueLogging.Standard.DAL.Cache
{
	public class HueLoggingCacheStateManager : IHueLoggingStateManager
	{
		private readonly IMemoryCache _cache;

		public HueLoggingCacheStateManager(IMemoryCache memoryCache)
		{
			_cache = memoryCache;
		}
		public async Task<LightEvent> GetLastEvent()
		{
			return await Task.Run(() => _cache.Get<LightEvent>("GetLastEvent"));
		}

		public async Task<LightEvent> GetLastEvent(string LightId)
		{
			return await Task.Run(() => _cache.Get<LightEvent>($"GetLastEvent.{LightId}"));
		}

		public async Task SaveAsLastLightEvent(LightEvent lightEvent)
		{
			await Task.Run(() => _cache.Set("GetLastEvent", lightEvent));
			await Task.Run(() => _cache.Set($"GetLastEvent.{lightEvent.Light.Id}", lightEvent));
		}
	}
}
