using HueLogging.Standard.DAL.Api;
using HueLogging.Standard.DAL.Cache;
using HueLogging.Standard.Library.Helpers.Comparers;
using HueLogging.Standard.Library.Helpers.Serializer;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HueLogging.Standard.Library.Extensions
{
	public static class HueLoggingServiceConfiguration
	{
		public static void AddHueLogging(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IBasicBinarySerializer, JsonSerializer>();
			serviceCollection.AddSingleton<IHueLoggingManager, HueLoggingManager>();
			serviceCollection.AddSingleton<IHueAccess, Q42HueAccess>();
			serviceCollection.AddSingleton<ActivityComparer>();
			serviceCollection.AddSingleton<IHueLoggingStateManager, HueLoggingCacheStateManager>();
			serviceCollection.AddMemoryCache();

		}
	}
}
