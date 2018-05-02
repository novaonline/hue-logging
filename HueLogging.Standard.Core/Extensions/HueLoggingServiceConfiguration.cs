using HueLogging.Standard.DAL.Api;
using HueLogging.Standard.DAL.Repository;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HueLogging.Standard.Library.Extensions
{
	public static class HueLoggingServiceConfiguration
	{
		public static void AddHueLogging(this IServiceCollection serviceCollection, System.Action<DbContextOptionsBuilder> contextOptions)
		{

			serviceCollection.AddSingleton<IHueLoggingManager, HueLoggingManager>();
			serviceCollection.AddSingleton<IHueAccess, Q42HueAccess>();
			serviceCollection.AddSingleton<IHueLoggingRepo, HueLogginRepo>();
			serviceCollection.AddDbContext<HueLoggingContext>(contextOptions, ServiceLifetime.Singleton, ServiceLifetime.Singleton);

		}
	}
}
