using HueLogging.Standard.Models.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HueLogging.ServiceV2
{
	internal class HueLoggingRecurringService : IHostedService, IDisposable
	{
		private readonly ILogger _logger;
		private readonly IHueLoggingManager _hueLoggingManager;
		private Timer _timer;
		private TimeSpan DefaultTimeSpan = TimeSpan.FromSeconds(20);
		private bool shouldReset = true;


		public HueLoggingRecurringService(ILogger<HueLoggingRecurringService> logger, IHueLoggingManager hueLoggingManager)
		{
			_logger = logger;
			_hueLoggingManager = hueLoggingManager;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("HueLogging Background Service is starting.");
			TimerCallback WorkDelegate = new TimerCallback(DoWork);
			_timer = new Timer(WorkDelegate, null, TimeSpan.Zero,
				DefaultTimeSpan);
			return Task.CompletedTask;
		}

		private void DoWork(object state)
		{
			_logger.LogInformation("Started HueLogging Instance");
			try
			{
				_hueLoggingManager.Start(shouldReset);
				shouldReset = false;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error Occured in one of the Hue Logging Instances");
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Timed Background Service is stopping.");

			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
