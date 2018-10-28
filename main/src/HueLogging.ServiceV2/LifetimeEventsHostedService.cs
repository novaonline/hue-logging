using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace HueLogging.ServiceV2
{
	public class LifetimeEventsHostedService : IHostedService
	{
		private readonly ILogger _logger;
		private readonly IApplicationLifetime _appLifetime;

		public LifetimeEventsHostedService(
			ILogger<LifetimeEventsHostedService> logger, IApplicationLifetime appLifetime)
		{
			_logger = logger;
			_appLifetime = appLifetime;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_appLifetime.ApplicationStarted.Register(OnStarted);
			_appLifetime.ApplicationStopping.Register(OnStopping);
			_appLifetime.ApplicationStopped.Register(OnStopped);

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private void OnStarted()
		{
			_logger.LogInformation("Starting Service");

			// Perform post-startup activities here
		}

		private void OnStopping()
		{
			_logger.LogInformation("Stopping Service");

			// Perform on-stopping activities here
		}

		private void OnStopped()
		{
			_logger.LogInformation("Good bye.");

			// Perform post-stopped activities here
		}
	}
}
